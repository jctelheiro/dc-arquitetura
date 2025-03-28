using ControleLancamentoAPI.Endpoints.LancamentosDiarios.V1;
using FluentValidation.TestHelper;
using FluxoCaixaContext;

namespace ControleLancamentoAPITests;

public class CreateLancamentoDiarioResquestValidatorTests
{
    private CreateLancamentoDiarioRequestValidator _validator = new ();
    
    [Fact]
    public void Validator_Nao_Deve_Retornar_Erros()
    {
        var model = new CreateLancamentoDiarioRequest(
            DateOnly.FromDateTime(DateTime.Today),
            DateOnly.FromDateTime(DateTime.Today.AddDays(1)), 
            "Teste", 
            12,
            123.89M,
            TipoLancamentoEnum.Credito);
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validator_Deve_Retornar_Erro_Ao_Validar_Data_Menor_Que_Atual()
    {
        var model = new CreateLancamentoDiarioRequest(
            DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),
            DateOnly.FromDateTime(DateTime.Today.AddDays(1)), 
            "Teste", 
            12,
            123.89M,
            TipoLancamentoEnum.Credito);
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(v => v.Data);
    }

    [Fact]
    public void Validator_Deve_Retornar_Erro_Ao_Validar_Data_Vencimento_Menor_Que_Data()
    {
        var model = new CreateLancamentoDiarioRequest(
            DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            DateOnly.FromDateTime(DateTime.Today.AddDays(-3)), 
            "Teste", 
            12,
            123.89M,
            TipoLancamentoEnum.Credito);
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(v => v.DataVencimento);
    }
    
    [Fact]
    public void Validator_Deve_Retornar_Erro_Ao_Validar_Valor_Menor_Ou_Igual_A_Zero()
    {
        var model = new CreateLancamentoDiarioRequest(
            DateOnly.FromDateTime(DateTime.Today),
            DateOnly.FromDateTime(DateTime.Today.AddDays(1)), 
            "Teste", 
            12,
            0M,
            TipoLancamentoEnum.Credito);
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(v => v.Valor);
    }
    
    [Fact]
    public void Validator_Deve_Retornar_Erro_Ao_Validar_Descricao_Maior_Que_250_Caracteres()
    {
        var model = new CreateLancamentoDiarioRequest(
            DateOnly.FromDateTime(DateTime.Today),
            DateOnly.FromDateTime(DateTime.Today.AddDays(1)), 
            new string( 'X', 251), 
            12,
            123.89M,
            TipoLancamentoEnum.Credito);
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(v => v.Descricao);
    }
    
    [Fact]
    public void Validator_Deve_Retornar_Erro_Ao_Validar_Descricao_Vazia()
    {
        var model = new CreateLancamentoDiarioRequest(
            DateOnly.FromDateTime(DateTime.Today),
            DateOnly.FromDateTime(DateTime.Today.AddDays(1)), 
            "", 
            12,
            123.89M,
            TipoLancamentoEnum.Credito);
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(v => v.Descricao);
    }
}