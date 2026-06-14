using Application.BooksApi.SearchBook;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Routes.BooksApi.SearchBook;

public class SearchBook
{
    public static async Task<Results<Ok<SearchBookResponse>, ProblemHttpResult>> Handle(
        IMediator mediator,
        string isbn
    )
    {
        var result = await mediator.Send(new SearchBookQuery(isbn));

        if (result.IsFailed)
            return TypedResults.Problem(
                string.Join(",", result.Errors.Select(x => x.Message)),
                statusCode: 500
            );

        var value = result.Value;

        return TypedResults.Ok(new SearchBookResponse(value.Title, value.Isbn, value.ThumbnailUrl));
    }
}
