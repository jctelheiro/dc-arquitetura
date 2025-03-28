using ConsolidadoDiarioServiceJob;
using FluxoCaixaContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ConsolidadoDiarioServiceJobTests;

public class ConsolidadoDiarioServiceTests : IClassFixture<DatabaseFixture>
{
    DatabaseFixture _fixture;

    public ConsolidadoDiarioServiceTests(DatabaseFixture databaseFixture)
    {
        _fixture = databaseFixture;
        _ = SeedDatabase(_fixture.Db);
    }
    
    private async Task SeedDatabase(FluxoCaixaDbContext ctx)
    {
        var lancamentos = new List<LancamentoDiario>()
        {
            new ()
            {
                Id = Guid.NewGuid(),
                ComercianteId = 12,
                Data = DateOnly.FromDateTime(DateTime.Today.AddDays(-5)),
                Tipo = TipoLancamentoEnum.Credito,
                Valor = 100M,
                DataVencimento = DateOnly.FromDateTime(DateTime.Today),
                Descricao = "Descricao"
            },
            new ()
            {
                Id = Guid.NewGuid(),
                ComercianteId = 12,
                Data = DateOnly.FromDateTime(DateTime.Today.AddDays(-4)),
                Tipo = TipoLancamentoEnum.Debito,
                Valor = 200M,
                DataVencimento = DateOnly.FromDateTime(DateTime.Today),
                Descricao = "Descricao"
            },
            new ()
            {
                Id = Guid.NewGuid(),
                ComercianteId = 12,
                Data = DateOnly.FromDateTime(DateTime.Today.AddDays(-4)),
                Tipo = TipoLancamentoEnum.Credito,
                Valor = 123M,
                DataVencimento = DateOnly.FromDateTime(DateTime.Today),
                Descricao = "Descricao"
            },
            new ()
            {
                Id = Guid.NewGuid(),
                ComercianteId = 12,
                Data = DateOnly.FromDateTime(DateTime.Today.AddDays(-3)),
                Tipo = TipoLancamentoEnum.Debito,
                Valor = 232M,
                DataVencimento = DateOnly.FromDateTime(DateTime.Today),
                Descricao = "Descricao"
            },
            new ()
            {
                Id = Guid.NewGuid(),
                ComercianteId = 12,
                Data = DateOnly.FromDateTime(DateTime.Today.AddDays(-2)),
                Tipo = TipoLancamentoEnum.Credito,
                Valor = 345.56M,
                DataVencimento = DateOnly.FromDateTime(DateTime.Today),
                Descricao = "Descricao"
            },
            new ()
            {
                Id = Guid.NewGuid(),
                ComercianteId = 12,
                Data = DateOnly.FromDateTime(DateTime.Today.AddDays(-2)),
                Tipo = TipoLancamentoEnum.Credito,
                Valor = 56M,
                DataVencimento = DateOnly.FromDateTime(DateTime.Today),
                Descricao = "Descricao"
            },
            new ()
            {
                Id = Guid.NewGuid(),
                ComercianteId = 12,
                Data = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),
                Tipo = TipoLancamentoEnum.Credito,
                Valor = 230M,
                DataVencimento = DateOnly.FromDateTime(DateTime.Today),
                Descricao = "Descricao"
            },
            new ()
            {
                Id = Guid.NewGuid(),
                ComercianteId = 12,
                Data = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),
                Tipo = TipoLancamentoEnum.Debito,
                Valor = 340M,
                DataVencimento = DateOnly.FromDateTime(DateTime.Today),
                Descricao = "Descricao"
            },
            new ()
            {
                Id = Guid.NewGuid(),
                ComercianteId = 12,
                Data = DateOnly.FromDateTime(DateTime.Today),
                Tipo = TipoLancamentoEnum.Credito,
                Valor = 100M,
                DataVencimento = DateOnly.FromDateTime(DateTime.Today),
                Descricao = "Descricao"
            },
            new ()
            {
                Id = Guid.NewGuid(),
                ComercianteId = 12,
                Data = DateOnly.FromDateTime(DateTime.Today.AddDays(-5)),
                Tipo = TipoLancamentoEnum.Debito,
                Valor = 450M,
                DataVencimento = DateOnly.FromDateTime(DateTime.Today),
                Descricao = "Descricao"
            },
            new ()
            {
                Id = Guid.NewGuid(),
                ComercianteId = 12,
                Data = DateOnly.FromDateTime(DateTime.Today),
                Tipo = TipoLancamentoEnum.Debito,
                Valor = 450M,
                DataVencimento = DateOnly.FromDateTime(DateTime.Today),
                Descricao = "Descricao"
            },
            new ()
            {
                Id = Guid.NewGuid(),
                ComercianteId = 11,
                Data = DateOnly.FromDateTime(DateTime.Today.AddDays(-5)),
                Tipo = TipoLancamentoEnum.Debito,
                Valor = 450M,
                DataVencimento = DateOnly.FromDateTime(DateTime.Today),
                Descricao = "Descricao"
            }
        };
        
        await _fixture.Db.LancamentosDiarios.AddRangeAsync(lancamentos);
        await _fixture.Db.SaveChangesAsync();   
    }
    
    [Fact]
    public async Task ConsolidadoDiarioService_ExecuteAsync_Success()
    {
        // Arrange
        using var loggerFactory = LoggerFactory.Create(b => { b.AddConsole(); });
        ILogger logger = loggerFactory.CreateLogger<ConsolidadoDiarioServiceTests>();
        var startEarlierDateProcessing = DateOnly.FromDateTime(DateTime.Today.AddDays(-15));
        var expectedCount = 6;
        
        // Act
        await ConsolidadoDiarioService.ExecuteJobAsync(logger, _fixture.Db, startEarlierDateProcessing);
        
        // Assert
        Assert.True(_fixture.Db.LancamentosDiarios.Any());
        Assert.Equal( await _fixture.Db.ConsolidadosDiarios.CountAsync(), expectedCount);
    }
}