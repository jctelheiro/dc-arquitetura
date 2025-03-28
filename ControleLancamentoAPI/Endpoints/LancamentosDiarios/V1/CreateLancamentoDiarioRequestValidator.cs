using FluentValidation;

namespace ControleLancamentoAPI.Endpoints.LancamentosDiarios.V1;

public class CreateLancamentoDiarioRequestValidator : AbstractValidator<CreateLancamentoDiarioRequest>
{
    public CreateLancamentoDiarioRequestValidator()
    {
        RuleFor(x => x.Data)
            .GreaterThanOrEqualTo(DateOnly.Parse(DateTime.Today.ToString("yyyy-MM-dd")))
            .WithMessage("A data deve ser maior ou igual a data atual.");
        
        RuleFor(x => x.Descricao)
            .MaximumLength(250)
            .NotEmpty()
            .WithMessage("O tamanho máximo é 250 caracteres ou não pode ser vazio.");

        RuleFor(x => x.Tipo)
            .IsInEnum()
            .WithMessage("O tipo está inválido.");
            
        RuleFor(x => x.DataVencimento)
            .Must((x, dataVencimento) => dataVencimento >= x.Data)
            .WithMessage("A data do vencimento deve ser maior ou igual a data do lançamento.");
        
        RuleFor(x => x.ComercianteId)
            .GreaterThan(0)
            .WithMessage("O id do comerciante deve ser maior que zero.");
        
        RuleFor(x => x.Valor)
            .GreaterThan(0)
            .WithMessage("O valor do lançamento deve ser maior que zero.");
    }
}