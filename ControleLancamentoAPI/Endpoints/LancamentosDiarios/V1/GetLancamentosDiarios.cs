using FluentValidation.Results;
using FluxoCaixaContext;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ControleLancamentoAPI.Endpoints.LancamentosDiarios.V1;

public static class GetLancamentosDiarios
{
    public static async Task<Results<Ok<LancamentoDiario[]>, ValidationProblem>> GetAsync(
        GetLancamentosDiariosFilter filter,
        FluxoCaixaDbContext ctx)
    {
        var validator = new GetLancamentosDiariosFilterValidator();
        ValidationResult validationResult = await validator.ValidateAsync(filter);
        
        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var queryFilter = ctx.LancamentosDiarios
            .AsNoTracking()
            .Where(q => q.ComercianteId == filter.ComercianteId);
        
        if (filter.DataFinal.HasValue)
        {
            queryFilter = queryFilter.Where(q => q.Data >= filter.DataInicial && q.Data <= filter.DataFinal.Value);
        }
        else
        {
            queryFilter = queryFilter.Where(q => q.Data == filter.DataInicial);
        }
        
        return TypedResults.Ok(await queryFilter.ToArrayAsync());
    }
}