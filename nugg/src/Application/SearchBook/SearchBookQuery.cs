using FluentResults;
using MediatR;

namespace Application.SearchBook;

public record SearchBookQuery(string Isbn) : IRequest<Result<SearchBookQueryResponse>>;