using FluxoCaixaContext;
using Microsoft.EntityFrameworkCore;

namespace Common;

public class MockDb : IDbContextFactory<FluxoCaixaDbContext>
{
    public FluxoCaixaDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<FluxoCaixaDbContext>()
            .UseInMemoryDatabase($"InMemoryTestDb-{DateTime.Now.ToFileTimeUtc()}")
            .Options;
        
        return new FluxoCaixaDbContext(options);
    }
}
