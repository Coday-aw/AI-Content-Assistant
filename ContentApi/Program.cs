using Microsoft.EntityFrameworkCore;
using K4U2.Data;
using K4U2.Interfaces;
using K4U2.Repositories;
using K4U2.Services;
using Polly;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IContentApiRepository, ContentApiRepository>();
builder.Services.AddHttpClient<IContentApiService, ContentApiService>(client =>
    {
        var config = builder.Configuration;
        client.BaseAddress = new Uri(config["ProxyApi:BaseUrl"]);
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
