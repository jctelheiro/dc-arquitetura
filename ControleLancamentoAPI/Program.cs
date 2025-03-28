using System.Text.Json.Serialization;
using ControleLancamentoAPI;
using FluxoCaixaContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = 
    builder.Configuration.GetConnectionString("FluxoCaixaContext")
    ?? throw new InvalidOperationException("No connection string found");

// OpenApi string Enum support
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<FluxoCaixaDbContext>(options =>
{
    options.UseNpgsql(connectionString);
    options.UseNpgsql(a => a.EnableRetryOnFailure());
});

var app = builder.Build();

// Global Exception Handling
app.UseExceptionHandler(exceptionHandlerApp 
    => exceptionHandlerApp.Run(async context 
        => await Results.Problem()
            .ExecuteAsync(context)));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();
app.ConfigureDatabase(connectionString);
app.ConfigureEndpoints();
app.Run();