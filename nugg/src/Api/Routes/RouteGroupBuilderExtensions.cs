using System.Diagnostics.CodeAnalysis;

namespace Api.Routes;

public static class RouteGroupBuilderExtensions
{
    
    public static RouteGroupBuilder MapPublicGroup(this IEndpointRouteBuilder endpoints,
        [StringSyntax("Route")] string prefix, string? groupTagName = null)
    {
        var group = endpoints.MapGroup(prefix);

        if (groupTagName != null)
            group.WithTags(groupTagName);

        group.WithOpenApi();

        return group;
    }
}
