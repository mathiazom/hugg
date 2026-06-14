using FluentResults;
using MediatR;

namespace Application.BooksApi.SearchBook;

public record SearchBookQuery(string Isbn) : IRequest<Result<SearchBookQueryResponse>>;
