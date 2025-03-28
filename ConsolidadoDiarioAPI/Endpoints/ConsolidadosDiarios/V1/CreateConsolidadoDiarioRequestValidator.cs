using FluentValidation;

namespace ConsolidadoDiarioAPI.Endpoints.ConsolidadosDiarios.V1;

public class CreateConsolidadoDiarioRequestValidator : AbstractValidator<CreateConsolidadoDiarioRequest>
{
    public CreateConsolidadoDiarioRequestValidator()
    {
        RuleFor(x => x.Data)
            .LessThan(DateOnly.Parse(DateTime.Today.ToString("yyyy-MM-dd")))
            .WithMessage("A data deve ser menor que a data atual.");
        
        RuleFor(x => x.ComercianteId)
            .GreaterThan(0)
            .WithMessage("O id do comerciante deve ser maior que zero.");
    }
}