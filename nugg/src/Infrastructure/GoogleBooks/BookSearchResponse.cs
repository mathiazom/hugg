namespace Infrastructure.GoogleBooks;

public record BookSearchResponse(List<BookSearchResponse.ItemsResponse> Items)
{
    public record ItemsResponse(ItemsResponse.VolumeInfoResponse VolumeInfo)
    {
        public record VolumeInfoResponse(
            string Title,
            List<VolumeInfoResponse.IndustryIdentifiersResponse> IndustryIdentifiers,
            VolumeInfoResponse.ImageLinksResponse ImageLinks
        )
        {
            public record IndustryIdentifiersResponse(string Type, string Identifier);

            public record ImageLinksResponse(string Thumbnail);
        }
    }
}
