namespace Api.Routes.BooksApi;

public static class BooksApiGroup
{
    public static WebApplication MapBooksApiGroup(this WebApplication app)
    {
        var group = app.MapPublicGroup("books-api");

        group.MapGet("/", SearchBook.SearchBook.Handle);

        return app;
    }
}
