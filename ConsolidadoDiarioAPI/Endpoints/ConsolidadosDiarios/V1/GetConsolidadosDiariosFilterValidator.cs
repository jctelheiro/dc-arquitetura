using FluentValidation;

namespace ConsolidadoDiarioAPI.Endpoints.ConsolidadosDiarios.V1;

public class GetConsolidadosDiariosFilterValidator : AbstractValidator<GetConsolidadosDiariosFilter>
{
    public GetConsolidadosDiariosFilterValidator()
    {
        var defaultDateOnly = default(DateOnly);
        
        RuleFor(x => x.ComercianteId)
            .GreaterThan(0)
            .WithMessage("O id do comerciante deve ser maior que zero.");
        
        RuleFor(x => x.DataInicial)
            .NotEqual(defaultDateOnly)
            .WithMessage("A data inicial deve ser informada.");
    }
}