using System.Net.Http.Json;
using Application.BooksApi;
using Application.BooksApi.DTO;
using Application.BooksApi.SearchBook;
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
        var httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri("https://www.googleapis.com");
        var apiKey = googleBooksConfig.Value.ApiKey;
        var httpResponseMessage = await httpClient.SendAsync(
            new HttpRequestMessage(
                HttpMethod.Get,
                $"/books/v1/volumes?q={isbn}&maxResults=10&key={apiKey}"
            ),
            cancellationToken
        );

        if (!httpResponseMessage.IsSuccessStatusCode)
            return Result.Fail("Google Books API request failed");

        var data = await httpResponseMessage.Content.ReadFromJsonAsync<BookSearchResponse>(
            cancellationToken: cancellationToken
        );

        if (data == null)
            return Result.Fail("Invalid Google Books API response");

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
}
