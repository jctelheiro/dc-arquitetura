using System.Diagnostics.CodeAnalysis;
using ConsolidadoDiarioAPI.Endpoints.ConsolidadosDiarios.V1;
using FluxoCaixaContext;
using Microsoft.EntityFrameworkCore;

namespace ConsolidadoDiarioAPI;

public static class ApplicationExtensions
{
    [ExcludeFromCodeCoverage]
    // Cria o Database caso não exista
    public static void ConfigureDatabase(this WebApplication app, string s)
    {
        var contextOptions = new DbContextOptionsBuilder<FluxoCaixaDbContext>()
            .UseNpgsql(s)
            .UseNpgsql(a => a.UseAdminDatabase("postgres"))
            .Options;

        using (var context = new FluxoCaixaDbContext(contextOptions)) 
        {
            context.Database.EnsureCreated();
        }
    }
    
    [ExcludeFromCodeCoverage]
    public static void ConfigureEndpoints(this WebApplication app)
    {
        var consolidadosDiarios = app.MapGroup("/v1/consolidadosdiarios");
        consolidadosDiarios.MapPost("/", CreateConsolidadosDiarios.PostAsync);
        consolidadosDiarios.MapGet("/", GetConsolidadosDiarios.GetAsync);
    }
}