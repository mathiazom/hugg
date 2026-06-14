using System.Net.Http.Json;
using Application.BooksApi;
using Application.BooksApi.DTO;
using FluentResults;
using Microsoft.Extensions.Options;

namespace Infrastructure.GoogleBooks;

public class GoogleBooksApiService(
    IHttpClientFactory httpClientFactory,
    IOptions<GoogleBooksConfig> googleBooksConfig
) : IBooksApiService
{
    public async Task<Result<BookSearchResultDto>> SearchBook(
        string isbn,
        CancellationToken cancellationToken = default
    )
    {
        var isbnSearchResponse = await SendBookSearchRequest(isbn, true, cancellationToken);

        if (!isbnSearchResponse.IsSuccessStatusCode)
            return Result.Fail("Google Books API ISBN search request failed");

        var data = await isbnSearchResponse.Content.ReadFromJsonAsync<BookSearchResponse>(
            cancellationToken: cancellationToken
        );

        if (data == null)
            return Result.Fail("Invalid Google Books API response for ISBN search");

        if (data.TotalItems == 0)
        {
            // try again without isbn keyword
            var genericSearchResponse = await SendBookSearchRequest(isbn, false, cancellationToken);

            if (!genericSearchResponse.IsSuccessStatusCode)
                return Result.Fail("Google Books API generic search request failed");

            data = await genericSearchResponse.Content.ReadFromJsonAsync<BookSearchResponse>(
                cancellationToken: cancellationToken
            );

            if (data == null)
                return Result.Fail("Invalid Google Books API response for generic search");
        }

        if (data.TotalItems == 0)
        {
            return Result.Fail("No book matches");
        }

        var firstResult = data.Items.First();
        var volumeInfo = firstResult.VolumeInfo;

        return Result.Ok(
            new BookSearchResultDto(
                volumeInfo.Title,
                volumeInfo.IndustryIdentifiers.First().Identifier,
                volumeInfo.ImageLinks.Thumbnail
            )
        );
    }

    private async Task<HttpResponseMessage> SendBookSearchRequest(
        string isbn,
        bool useIsbnKeyword,
        CancellationToken cancellationToken = default
    )
    {
        var httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri("https://www.googleapis.com");
        var apiKey = googleBooksConfig.Value.ApiKey;
        var query = (useIsbnKeyword ? "isbn:" : "") + isbn;
        var httpResponseMessage = await httpClient.SendAsync(
            new HttpRequestMessage(
                HttpMethod.Get,
                $"/books/v1/volumes?q={query}&maxResults=1&key={apiKey}"
            ),
            cancellationToken
        );
        return httpResponseMessage;
    }
}
