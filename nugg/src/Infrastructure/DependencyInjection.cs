using Application;
using Application.BooksApi;
using Infrastructure.GoogleBooks;
using Infrastructure.TestContainers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder
            .AddInfrastructureConfig()
            .AddTestContainersConfig(out var currentTestContainersConfig);

        if (currentTestContainersConfig.Enabled)
        {
            builder.AddTestContainers();
        }

        builder.Services.AddDbContext<DatabaseContext>();

        builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<DatabaseContext>());

        builder.Services.AddScoped<IBooksApiService, GoogleBooksApiService>();

        return builder;
    }

    public static IHealthChecksBuilder AddInfrastructureHealthChecks(
        this IHealthChecksBuilder healthChecksBuilder
    )
    {
        healthChecksBuilder.AddDbContextCheck<DatabaseContext>();

        return healthChecksBuilder;
    }
}
