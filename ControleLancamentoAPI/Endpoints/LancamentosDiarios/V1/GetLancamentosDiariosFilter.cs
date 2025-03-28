using System.Reflection;

namespace ControleLancamentoAPI.Endpoints.LancamentosDiarios.V1;

public class GetLancamentosDiariosFilter 
{
    public int ComercianteId { get; init; }
    public DateOnly DataInicial { get; init; }
    public DateOnly? DataFinal { get; init; }

    public static ValueTask<GetLancamentosDiariosFilter?> BindAsync(
        HttpContext context,
        ParameterInfo parameter)
    {
        const string comercianteKey = "comercianteId";
        const string dataInicialKey = "dataInicial";
        const string dataFinalKey = "dataFinal";

        int.TryParse(context.Request.Query[comercianteKey], out var comercianteId);
        DateOnly.TryParse(context.Request.Query[dataInicialKey], out var dataInicial);
        DateOnly.TryParse(context.Request.Query[dataFinalKey], out var dataFinal);

        var result = new GetLancamentosDiariosFilter
        {
            ComercianteId = comercianteId,
            DataInicial = dataInicial,
            DataFinal = dataFinal
        };

        return ValueTask.FromResult<GetLancamentosDiariosFilter?>(result);
    }
}