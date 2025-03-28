namespace FluxoCaixaContext;

public class LancamentoDiario
{
    public Guid Id { get; init; }
    public TipoLancamentoEnum Tipo { get; init; }
    public decimal Valor { get; init; }
    public DateOnly Data { get; init; }  
    public DateOnly DataVencimento { get; init; }
    public string Descricao { get; init; }
    public int ComercianteId { get; init; }
}