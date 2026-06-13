using Api.Authorization;
using Api.Routes.BooksApi;
using Application;
using FluentValidation;
using Infrastructure;
using Serilog;

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
    {
        options.AddPolicy(
            name: myAllowSpecificOrigins,
            policy => { policy.WithOrigins("*"); }
        );
    }
);

// Add other layers
builder.AddApplication();
builder.AddInfrastructure();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Add JWT-token authentication + our custom authorization policies
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization(options => { options.AddAuthorizationPolicies(); });

// FluentValidation register all validators present in this assembly
builder.Services.AddValidatorsFromAssemblyContaining<Api.Program>();

// Add Logging
builder.Host.UseSerilog((context, services, config) => config
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
);

// Add ProblemDetails for error handling of all non-problem error responses
builder.Services.AddProblemDetails();

builder.Services.AddHealthChecks()
    .AddInfrastructureHealthChecks();

builder.Services.AddHttpClient();

var app = builder.Build();

app.UseCors(myAllowSpecificOrigins);

// Produce a ProblemDetails payload for exceptions
app.UseExceptionHandler();

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/healthz");

app.MapBooksApiGroup();

app.Run();

// To make it visible for E2E-tests:
namespace Api
{
    public partial class Program
    {
    }
}