using FluentValidation.Results;
using FluxoCaixaContext;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ConsolidadoDiarioAPI.Endpoints.ConsolidadosDiarios.V1;

public static class GetConsolidadosDiarios
{
    public static async Task<Results<Ok<ConsolidadoDiario[]>, ValidationProblem>> GetAsync(
        GetConsolidadosDiariosFilter filter,
        FluxoCaixaDbContext ctx)
    {
        var validator = new GetConsolidadosDiariosFilterValidator();
        ValidationResult validationResult = await validator.ValidateAsync(filter);
        
        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }

        var queryFilter = ctx.ConsolidadosDiarios
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