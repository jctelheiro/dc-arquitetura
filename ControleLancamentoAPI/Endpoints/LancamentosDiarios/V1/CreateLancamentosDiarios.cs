using FluentValidation.Results;
using FluxoCaixaContext;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ControleLancamentoAPI.Endpoints.LancamentosDiarios.V1;

public static class CreateLancamentosDiarios
{
    public static async Task<Results<Created<LancamentoDiario>, ValidationProblem, ProblemHttpResult>> PostAsync(
        CreateLancamentoDiarioRequest lancamento, 
        FluxoCaixaDbContext ctx)
    {
        var validator = new CreateLancamentoDiarioRequestValidator();
        ValidationResult validationResult = await validator.ValidateAsync(lancamento);
        
        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary());
        }
        
        LancamentoDiario lancamentoDiario = new()
        {
            Id = Guid.NewGuid(),
            Descricao = lancamento.Descricao,
            ComercianteId = lancamento.ComercianteId,
            DataVencimento = lancamento.DataVencimento,
            Data = lancamento.Data,
            Tipo = lancamento.Tipo,
            Valor = lancamento.Valor,
        };
    
        await ctx.LancamentosDiarios.AddAsync(lancamentoDiario);

        try
        {
            await ctx.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            return TypedResults.Problem(exception.Message);
        }

        return TypedResults.Created($"/v1/lancamentosdiarios/{lancamentoDiario.Id.ToString()}", lancamentoDiario);
    }
}