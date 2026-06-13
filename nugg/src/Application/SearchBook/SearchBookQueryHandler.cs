using System.Net.Http.Json;
using System.Text.Json;
using FluentResults;
using MediatR;

namespace Application.SearchBook;

public record BooksApiSearchResponse(List<BooksApiSearchResponse.ItemsResponse> Items)
{
    public record ItemsResponse(ItemsResponse.VolumeInfoResponse VolumeInfo)
    {
        public record VolumeInfoResponse(
            string Title,
            List<VolumeInfoResponse.IndustryIdentifiersResponse> IndustryIdentifiers,
            VolumeInfoResponse.ImageLinksResponse ImageLinks)
        {
            public record IndustryIdentifiersResponse(string Type, string Identifier);

            public record ImageLinksResponse(string Thumbnail);
        }
    }
}

internal class SearchBookQueryHandler(IHttpClientFactory httpClientFactory)
    : IRequestHandler<SearchBookQuery, Result<SearchBookQueryResponse>>
{
    public async Task<Result<SearchBookQueryResponse>> Handle(SearchBookQuery request,
        CancellationToken cancellationToken)
    {
        var httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri("https://www.googleapis.com");
        var httpResponseMessage = await httpClient.SendAsync(
            new HttpRequestMessage(
                HttpMethod.Get,
                "/books/v1/volumes?q=9788282262019&maxResults=10&key=AIzaSyAdXFSHVopfJ7sW8JuMr0I1yynVxp1HzHc"
            ),
            cancellationToken
        );

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var data =
                await httpResponseMessage.Content.ReadFromJsonAsync<BooksApiSearchResponse>(cancellationToken: cancellationToken);

            if (data == null) return Result.Fail("Failed");

            var firstResult = data.Items.First();
            var volumeInfo = firstResult.VolumeInfo;
            
            return Result.Ok(
                new SearchBookQueryResponse(
                    volumeInfo.Title,
                    volumeInfo.IndustryIdentifiers.First().Identifier,
                    volumeInfo.ImageLinks.Thumbnail
                )
            );
        }

        return Result.Fail("Failed");
    }
}