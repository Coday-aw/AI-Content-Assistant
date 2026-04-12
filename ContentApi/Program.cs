using Microsoft.EntityFrameworkCore;
using K4U2.Data;
using K4U2.Exceptions;
using K4U2.Interfaces;
using K4U2.Repositories;
using K4U2.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IContentApiRepository, ContentApiRepository>();
builder.Services.AddHttpClient<IContentApiService, ContentApiService>(client =>
    {
        var config = builder.Configuration;
        client.BaseAddress = new Uri(config["ProxyApi:BaseUrl"] ?? throw new InvalidOperationException("ProxyApi.BaseUrl is missing"));
    })
    .AddPolicyHandler(Policy<HttpResponseMessage>
        .Handle<HttpRequestException>()
        .WaitAndRetryAsync(
            retryCount: 3,
            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt - 1))));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

app.UseExceptionHandler(exceptionHandler =>
{
    exceptionHandler.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        var problemDetails = exception switch
        {
            ProxyApiUnavailableException ex => new ProblemDetails
            {
                Status = 503,
                Title = "Api is unavailable",
                Detail = ex.Message,
            },
            NotFoundException ex => new ProblemDetails
            {
              Status  = 404,
              Title = "Not found",
              Detail = ex.Message,
            },
            _ => new ProblemDetails
            {
                Status = 500,
                Title = "Internal Server Error",
                Detail = "Internal Server Error, try again later.",
            }
        };
        
        context.Response.StatusCode = problemDetails.Status ?? 500;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(problemDetails);
    });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
