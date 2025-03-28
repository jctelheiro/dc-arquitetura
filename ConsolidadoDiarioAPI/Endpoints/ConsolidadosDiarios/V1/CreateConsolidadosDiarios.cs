using FluentValidation.Results;
using FluxoCaixaContext;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ConsolidadoDiarioAPI.Endpoints.ConsolidadosDiarios.V1;

public static class CreateConsolidadosDiarios
{
    public static async Task<Results<Created<ConsolidadoDiario>, ValidationProblem, ProblemHttpResult>> PostAsync(
        CreateConsolidadoDiarioRequest consolidado,
        FluxoCaixaDbContext ctx, 
        CancellationToken cancellationToken = default)
    {
        var validator = new CreateConsolidadoDiarioRequestValidator();
        ValidationResult validationResult = await validator.ValidateAsync(consolidado);
        
        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }
        
        ConsolidadoDiario consolidadoDiario = new()
        {
            SaldoFinal =  consolidado.SaldoFinal,
            ComercianteId = consolidado.ComercianteId,
            SaldoInicial =  consolidado.SaldoInicial,
            Data = consolidado.Data
        };
    
        await ctx.ConsolidadosDiarios.AddAsync(consolidadoDiario, cancellationToken);

        try
        {
            await ctx.SaveChangesAsync(cancellationToken);
        }
        catch (Exception exception)
        {
            return TypedResults.Problem(exception.Message);
        }

        return TypedResults.Created<ConsolidadoDiario>($"/v1/consolidadosdiarios/{consolidadoDiario.Id}", consolidadoDiario);
    }
}