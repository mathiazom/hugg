using FluentResults;
using MediatR;

namespace Application.BooksApi.SearchBook;

internal class SearchBookQueryHandler(IBooksApiService booksApiService)
    : IRequestHandler<SearchBookQuery, Result<SearchBookQueryResponse>>
{
    public async Task<Result<SearchBookQueryResponse>> Handle(
        SearchBookQuery request,
        CancellationToken cancellationToken
    )
    {
        var result = await booksApiService.SearchBook(request.Isbn, cancellationToken);

        if (result.IsFailed)
        {
            return Result.Fail("Failed");
        }

        var book = result.Value;

        return Result.Ok(new SearchBookQueryResponse(book.Title, book.Isbn, book.ThumbnailUrl));
    }
}
