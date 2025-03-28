using Common;
using ControleLancamentoAPI.Endpoints.LancamentosDiarios.V1;
using FluxoCaixaContext;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Moq.EntityFrameworkCore;

namespace ControleLancamentoAPITests;

public class CreateLancamentosDiariosTests
{
    [Fact]
    public async Task CreateLancamentosDiarios_Com_Sucesso_Retorna_Created()
    {
        // Arrange
        await using var ctx = new MockDb().CreateDbContext();
        
        var request = new CreateLancamentoDiarioRequest(
            DateOnly.FromDateTime(DateTime.Today), 
            DateOnly.FromDateTime(DateTime.Today),
            "Teste", 
            12,
            123.89M,
            TipoLancamentoEnum.Credito);
        
        //Act
        var result = await CreateLancamentosDiarios.PostAsync(request, ctx);
        
        //Assert
        Assert.IsType<Created<LancamentoDiario>>(result.Result);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task CreateLancamentosDiarios_Codigo_Comerciante_Invalido_Retorna_Validation_Problem()
    {
        // Arrange
        await using var ctx = new MockDb().CreateDbContext();
        
        var request = new CreateLancamentoDiarioRequest(
            DateOnly.FromDateTime(DateTime.Today), 
            DateOnly.FromDateTime(DateTime.Today),
            "Teste", 
            0,
            123.89M,
            TipoLancamentoEnum.Credito);
        
        //Act
        var result = await CreateLancamentosDiarios.PostAsync(request, ctx);
        
        //Assert
        Assert.IsType<ValidationProblem>(result.Result);
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task CreateLancamentosDiarios_Request_Data_Invalida_Retorna_Validation_Problem()
    {
        // Arrange
        await using var ctx = new MockDb().CreateDbContext();
        
        var request = new CreateLancamentoDiarioRequest(
            DateOnly.FromDateTime(DateTime.Today.AddDays(-1)), 
            DateOnly.FromDateTime(DateTime.Today),
            "Teste", 
            12,
            123.89M,
            TipoLancamentoEnum.Credito);
        
        //Act
        var result = await CreateLancamentosDiarios.PostAsync(request, ctx);
        
        //Assert
        Assert.IsType<ValidationProblem>(result.Result);
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task CreateConsolidadosDiarios_Retorna_Problem_Quando_Exception_Ocorrer()
    {
        // Arrange
        var ctx = new Mock<FluxoCaixaDbContext>();
        ctx.Setup(x => x.LancamentosDiarios).ReturnsDbSet(new List<LancamentoDiario>());
        ctx.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
        
        var request = new CreateLancamentoDiarioRequest(
            DateOnly.FromDateTime(DateTime.Today), 
            DateOnly.FromDateTime(DateTime.Today),
            "Teste", 
            12,
            123.89M,
            TipoLancamentoEnum.Credito);
        
        //Act
        var result = await CreateLancamentosDiarios.PostAsync(request, ctx.Object);
        
        //Assert
        Assert.IsType<ProblemHttpResult>(result.Result);
        Assert.NotNull(result);
    }
}