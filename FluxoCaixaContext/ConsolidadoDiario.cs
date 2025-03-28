namespace FluxoCaixaContext;

public class ConsolidadoDiario
{
    public Guid Id { get; init; }
    public DateOnly Data { get; init; }
    public decimal SaldoInicial { get; init; }
    public int ComercianteId { get; init; }
    public decimal SaldoFinal { get; init; }
}