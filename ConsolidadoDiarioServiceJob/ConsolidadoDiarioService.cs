using FluxoCaixaContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ConsolidadoDiarioServiceJob;

public static class ConsolidadoDiarioService
{
    public static async Task ExecuteJobAsync(
        ILogger logger,
        FluxoCaixaDbContext ctx,
        DateOnly startEarlierDateProcessing)
    {
        var limitDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));
        
        var lancamentosAgrupados = ctx.LancamentosDiarios
            .AsNoTracking()
            .Where(c => c.Data >= startEarlierDateProcessing && c.Data <= limitDate)
            .GroupBy(g => new { g.Data, g.ComercianteId });

        var processados = 0;
        
        foreach (var item in lancamentosAgrupados)
        {
            var processado = await ctx.ConsolidadosDiarios
                .AsNoTracking()
                .AnyAsync(c => c.ComercianteId == item.Key.ComercianteId && c.Data == item.Key.Data);

            if (!processado)
            {
                logger.LogInformation($"Consolidado Diário: comercianteId = '{item.Key.ComercianteId}', data = '{item.Key.Data}' - iniciado");
                
                try
                {
                    await ConsolidarDiarioAsync(ctx, item.Key.ComercianteId, item.Key.Data);
                }
                catch (Exception e)
                {
                    logger.LogError($"Consolidado Diário: comercianteId = '{item.Key.ComercianteId}', data = '{item.Key.Data}' - Erro ao inserir no banco de dados: {e.Message}");
                }
                
                logger.LogInformation($"Consolidado Diário: comercianteId = '{item.Key.ComercianteId}', data = '{item.Key.Data}' - processado");
                
                processados++;
            }
            else
            {
                logger.LogInformation(
                    $"Consolidado Diário: comercianteId = '{item.Key.ComercianteId}', data = '{item.Key.Data}' - já estava processado");
            }
        }
        
        logger.LogInformation($"Consolidados Processados: {processados}");
    }

    private static async Task ConsolidarDiarioAsync(
        FluxoCaixaDbContext ctx, 
        int comercianteId, 
        DateOnly dataConsolidacao)
    {
        var saldoInicial = await CalcularSaldoInicial(ctx, comercianteId, dataConsolidacao);
        var saldoFinal = await CalcularSaldoFinalAsync(ctx, saldoInicial, comercianteId, dataConsolidacao); 
        
        var consolidado = new ConsolidadoDiario()
        {
            Id = Guid.NewGuid(),
            Data = dataConsolidacao,
            ComercianteId = comercianteId,
            SaldoInicial = saldoInicial,
            SaldoFinal = saldoFinal
        };

        await ctx.ConsolidadosDiarios.AddAsync(consolidado);
        await ctx.SaveChangesAsync();
    }

    private static async Task<decimal> CalcularSaldoInicial(
        FluxoCaixaDbContext ctx, 
        int comercianteId, 
        DateOnly dataConsolidacao)
    {
        var ultimaConsolidacao = await ctx.ConsolidadosDiarios
            .AsNoTracking()
            .OrderByDescending(o => o.Data)
            .FirstOrDefaultAsync(s => s.ComercianteId == comercianteId && s.Data <= dataConsolidacao);
        
        var saldoInicial = ultimaConsolidacao?.SaldoFinal ?? 0M;
        
        return saldoInicial;
    }

    private static async Task<decimal> CalcularSaldoFinalAsync(
        FluxoCaixaDbContext ctx, 
        decimal saldoInicial, 
        int comercianteId, 
        DateOnly dataConsolidacao)
    {
        var consolidados = from l in ctx.LancamentosDiarios.AsNoTracking()
            where l.ComercianteId == comercianteId && l.Data.Equals(dataConsolidacao)
            group l by new { l.Tipo }
            into grp
            select new
            {
                grp.Key.Tipo,
                Total = grp.Sum(x => x.Valor)
            };
        
        var saldoFinal = saldoInicial;
        
        foreach (var item in await consolidados.ToListAsync())
        {
            if (item.Tipo == TipoLancamentoEnum.Debito)
                saldoFinal -= item.Total;
            else 
                saldoFinal += item.Total;
        }

        return saldoFinal;
    }
}