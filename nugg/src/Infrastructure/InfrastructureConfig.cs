using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure;

public sealed record InfrastructureConfig
{
    public const string SectionName = nameof(InfrastructureConfig);

    public required string ConnectionString { get; set; }
    public required bool EnableSensitiveDataLogging { get; set; }
}

public sealed record GoogleBooksConfig
{
    public const string SectionName = nameof(GoogleBooksConfig);

    public required string ApiKey { get; set; }
}

internal static class InfrastructureConfigExtensions
{
    internal static IHostApplicationBuilder AddInfrastructureConfig(
        this IHostApplicationBuilder builder
    )
    {
        builder.Services.Configure<InfrastructureConfig>(
            builder.Configuration.GetSection(InfrastructureConfig.SectionName)
        );
        builder.Services.Configure<GoogleBooksConfig>(
            builder.Configuration.GetSection(GoogleBooksConfig.SectionName)
        );
        return builder;
    }
}
