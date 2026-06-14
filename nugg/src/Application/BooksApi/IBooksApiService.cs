using Application.BooksApi.DTO;
using FluentResults;

namespace Application.BooksApi;

public interface IBooksApiService
{
    public Task<Result<BookSearchResultDto>> SearchBook(
        string isbn,
        CancellationToken cancellationToken = default
    );
}
