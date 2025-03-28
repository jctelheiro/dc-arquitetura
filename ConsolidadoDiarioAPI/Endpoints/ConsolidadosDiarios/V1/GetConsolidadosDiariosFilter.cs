using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace ConsolidadoDiarioAPI.Endpoints.ConsolidadosDiarios.V1;

[ExcludeFromCodeCoverage]
public class GetConsolidadosDiariosFilter
{
    public int ComercianteId { get; init; }
    public DateOnly DataInicial { get; init; }
    public DateOnly? DataFinal { get; init; }

    public static ValueTask<GetConsolidadosDiariosFilter?> BindAsync(
        HttpContext context,
        ParameterInfo parameter
        )
    {
        const string comercianteKey = "comercianteId";
        const string dataInicialKey = "dataInicial";
        const string dataFinalKey = "dataFinal";
        
        int.TryParse(context.Request.Query[comercianteKey], out var comercianteId);
        DateOnly.TryParse(context.Request.Query[dataInicialKey], out var dataInicial);
        DateOnly.TryParse(context.Request.Query[dataFinalKey], out var dataFinal);
        
        var result = new GetConsolidadosDiariosFilter
        {
            ComercianteId  = comercianteId,
            DataInicial = dataInicial,
            DataFinal = dataFinal
        };

        return ValueTask.FromResult<GetConsolidadosDiariosFilter?>(result);
    }
}
