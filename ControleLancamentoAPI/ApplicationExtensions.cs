using ControleLancamentoAPI.Endpoints.LancamentosDiarios.V1;
using FluxoCaixaContext;
using Microsoft.EntityFrameworkCore;

namespace ControleLancamentoAPI;

public static class ApplicationExtensions
{
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
    
    public static void ConfigureEndpoints(this WebApplication app)
    {
        var lancamentosDiarios = app.MapGroup("/v1/lancamentosdiarios");
        lancamentosDiarios.MapPost("/", CreateLancamentosDiarios.PostAsync);
        lancamentosDiarios.MapGet("/", GetLancamentosDiarios.GetAsync);
    }
}