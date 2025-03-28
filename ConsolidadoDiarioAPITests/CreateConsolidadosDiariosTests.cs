using Common;
using ConsolidadoDiarioAPI.Endpoints.ConsolidadosDiarios.V1;
using FluxoCaixaContext;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Moq.EntityFrameworkCore;

namespace ConsolidadoDiarioAPITests;

public class CreateConsolidadosDiariosTests
{
    [Fact]
    public async Task CreateConsolidadosDiarios_Com_Sucesso_Retorna_Created()
    {
        // Arrange
        await using var ctx = new MockDb().CreateDbContext();
        
        CreateConsolidadoDiarioRequest consolidadoDiarioRequest = new(
            DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),
            123M,
            12,
            -12M); 
        
        //Act
        var result = await CreateConsolidadosDiarios.PostAsync(consolidadoDiarioRequest, ctx);
        
        //Assert
        Assert.IsType<Created<ConsolidadoDiario>>(result.Result);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task CreateConsolidadosDiarios_Codigo_Comerciante_Invalido_Retorna_Validation_Problem()
    {
        // Arrange
        await using var ctx = new MockDb().CreateDbContext();
        
        CreateConsolidadoDiarioRequest consolidadoDiarioRequest = new(
            DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),
            123M,
            0,
            -12M); 
        
        //Act
        var result = await CreateConsolidadosDiarios.PostAsync(consolidadoDiarioRequest, ctx);
        
        //Assert
        Assert.IsType<ValidationProblem>(result.Result);
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task CreateConsolidadosDiarios_Data_Invalida_Retorna_Validation_Problem()
    {
        // Arrange
        await using var ctx = new MockDb().CreateDbContext();
        
        CreateConsolidadoDiarioRequest consolidadoDiarioRequest = new(
            DateOnly.FromDateTime(DateTime.Today),
            123M,
            12,
            -12M); 
        
        //Act
        var result = await CreateConsolidadosDiarios.PostAsync(consolidadoDiarioRequest, ctx);
        
        //Assert
        Assert.IsType<ValidationProblem>(result.Result);
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task CreateConsolidadosDiarios_Retorna_Problem_Quando_Exception_Ocorrer()
    {
        // Arrange
        var ctx = new Mock<FluxoCaixaDbContext>();
        ctx.Setup(x => x.ConsolidadosDiarios).ReturnsDbSet(new List<ConsolidadoDiario>());
        ctx.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
        
        CreateConsolidadoDiarioRequest consolidadoDiarioRequest = new(
            DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),
            123M,
            12,
            -12M);
        
        //Act
        var result = await CreateConsolidadosDiarios.PostAsync(consolidadoDiarioRequest, ctx.Object);
        
        //Assert
        Assert.IsType<ProblemHttpResult>(result.Result);
        Assert.NotNull(result);
    }
}