using FluxoCaixaContext;
using Microsoft.EntityFrameworkCore;

namespace ConsolidadoDiarioServiceJobTests;

public class DatabaseFixture : IDisposable
{
    public DatabaseFixture()
    {
        var connectionString = Environment.GetEnvironmentVariable("FLUXOCAIXA_CONNECTION_STRING");

        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = $"Host=postgres;Username=root;Password=secret;Database=ConsolidadoDiarioServiceJobTest-{DateTime.Now.ToString("yyyyMMddHHmmss")}";
        }
        DbContextOptionsBuilder<FluxoCaixaDbContext> builder = new();
        builder.UseNpgsql(connectionString);
        builder.UseNpgsql(a => a.EnableRetryOnFailure());
        
        var dbContext = new FluxoCaixaDbContext(builder.Options);
        dbContext.Database.EnsureCreated();
        Db =  dbContext;
    }

    public void Dispose()
    {
        Db.Database.EnsureDeleted();
        Db.Dispose();
    }

    public FluxoCaixaDbContext Db { get; private set; }
}