using System.Diagnostics.CodeAnalysis;

namespace ConsolidadoDiarioAPI.Endpoints.ConsolidadosDiarios.V1;

[ExcludeFromCodeCoverage]
public record CreateConsolidadoDiarioRequest(
    DateOnly Data,
    decimal SaldoInicial,
    int ComercianteId, 
    decimal SaldoFinal);