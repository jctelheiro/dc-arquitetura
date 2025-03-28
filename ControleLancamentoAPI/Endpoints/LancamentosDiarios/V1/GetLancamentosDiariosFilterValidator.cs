using FluentValidation;

namespace ControleLancamentoAPI.Endpoints.LancamentosDiarios.V1;

public class GetLancamentosDiariosFilterValidator : AbstractValidator<GetLancamentosDiariosFilter>
{
    public GetLancamentosDiariosFilterValidator()
    {
        var defaultDateOnly = default(DateOnly);
        
        RuleFor(x => x.ComercianteId)
            .GreaterThan(0)
            .WithMessage("O parâmetro 'comercianteid' deve ser maior que zero.");
        
        RuleFor(x => x.DataInicial)
            .NotEqual(defaultDateOnly)
            .WithMessage("O parâmetro 'datainicial' deve ser informada.");
    }
}