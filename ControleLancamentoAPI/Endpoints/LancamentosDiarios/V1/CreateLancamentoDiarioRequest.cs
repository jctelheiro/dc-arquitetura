using System.Diagnostics.CodeAnalysis;
using FluxoCaixaContext;

namespace ControleLancamentoAPI.Endpoints.LancamentosDiarios.V1;

[ExcludeFromCodeCoverage]
public record CreateLancamentoDiarioRequest(
    DateOnly Data,
    DateOnly DataVencimento,
    string Descricao,
    int ComercianteId,
    decimal Valor,
    TipoLancamentoEnum Tipo); 
