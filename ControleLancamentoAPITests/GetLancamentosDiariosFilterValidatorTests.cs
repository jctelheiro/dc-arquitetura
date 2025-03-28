using ControleLancamentoAPI.Endpoints.LancamentosDiarios.V1;
using FluentValidation.TestHelper;

namespace ControleLancamentoAPITests;

public class GetLancamentosDiariosFilterValidatorTests
{
    private GetLancamentosDiariosFilterValidator _validator = new();

    [Fact]
    public void Validator_Nao_Deve_Retornar_Erros()
    {
        var model = new GetLancamentosDiariosFilter 
        {
            ComercianteId = 12,
            DataInicial = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),
            DataFinal = DateOnly.FromDateTime(DateTime.Today)
        };
        
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validator_Deve_Retornar_Erro_Ao_Validar_Data_Inicial_Default()
    {
        var model = new GetLancamentosDiariosFilter 
        {
            ComercianteId = 12,
            DataInicial = default(DateOnly),
            DataFinal = null
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(v => v.DataInicial);
    }
    
    [Fact]
    public void Validator_Deve_Retornar_Ao_Validar_ComercianteId_Menor_Ou_Igual_A_Zero()
    {
        var model = new GetLancamentosDiariosFilter 
        { 
            ComercianteId = 0,
            DataInicial = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),
            DataFinal = null
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(v => v.ComercianteId);
    }
}