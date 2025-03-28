using ConsolidadoDiarioAPI.Endpoints.ConsolidadosDiarios.V1;
using FluentValidation.TestHelper;

namespace ConsolidadoDiarioAPITests;

public class CreateConsolidadoDiarioRequestValidatorTests
{
    private CreateConsolidadoDiarioRequestValidator _validator = new ();
    
    [Fact]
    public void Validator_Nao_Deve_Retornar_Erros()
    {
        var model = new CreateConsolidadoDiarioRequest(
            DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),
            12.00M, 
            12, 
            34.00M);
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validator_Deve_Retornar_Erro_Ao_Validar_Data_Igual_A_Atual()
    {
        var model = new CreateConsolidadoDiarioRequest(
            DateOnly.FromDateTime(DateTime.Today),
            12.00M, 
            12, 
            34.00M);
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(v => v.Data);
    }

    [Fact]
    public void Validator_Deve_Retornar_Erro_Ao_Validar_ComercianteId_Menor_Ou_Igual_A_Zero()
    {
        var model = new CreateConsolidadoDiarioRequest(
            DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),
            12.00M, 
            0, 
            34.00M);
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(v => v.ComercianteId);
    }
}