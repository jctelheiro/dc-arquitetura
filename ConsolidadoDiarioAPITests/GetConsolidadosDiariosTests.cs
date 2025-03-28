using ConsolidadoDiarioAPI.Endpoints.ConsolidadosDiarios.V1;
using FluxoCaixaContext;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Moq.EntityFrameworkCore;

namespace ConsolidadoDiarioAPITests;

public class GetConsolidadosDiariosTests
{
    [Fact]
    public async Task GetConsolidadosDiarios_Com_Sucesso_Retorna_Lista()
    {
        // Arrange
        var ctx = new Mock<FluxoCaixaDbContext>();
        var consolidadosList = new List<ConsolidadoDiario>()
        {
            new ()
            {
                Id = Guid.NewGuid(),
                ComercianteId = 12,
                Data = DateOnly.Parse("2025-03-21"),
                SaldoInicial = 12M,
                SaldoFinal = 112M
            },
            new ()
            {
                Id = Guid.NewGuid(),
                ComercianteId = 12,
                Data = DateOnly.Parse("2025-03-22"),
                SaldoInicial = 32.91M,
                SaldoFinal = -92M
            },
            new ()
            {
                Id = Guid.NewGuid(),
                ComercianteId = 12,
                Data = DateOnly.Parse("2025-03-23"),
                SaldoInicial = 212M,
                SaldoFinal = 1212M
            },
            new ()
            {
                Id = Guid.NewGuid(),
                ComercianteId = 12,
                Data = DateOnly.Parse("2025-03-24"),
                SaldoInicial = 622.67M,
                SaldoFinal = -1112M
            },
            new ()
            {
                Id = Guid.NewGuid(),
                ComercianteId = 12,
                Data = DateOnly.Parse("2025-03-25"),
                SaldoInicial = 43M,
                SaldoFinal = 789M
            },
            new ()
            {
                Id = Guid.NewGuid(),
                ComercianteId = 11,
                Data = DateOnly.Parse("2025-03-26"),
                SaldoInicial = 12M,
                SaldoFinal = 112M
            },
        };
        ctx.Setup(x => x.ConsolidadosDiarios).ReturnsDbSet(consolidadosList);

        GetConsolidadosDiariosFilter filter = new()
        {
            ComercianteId = 12,
            DataInicial = DateOnly.Parse("2025-03-21"),
            DataFinal = DateOnly.Parse("2025-03-26")
        }; 
        
        //Act
        var result = await GetConsolidadosDiarios.GetAsync(filter, ctx.Object);
        
        //Assert
        Assert.IsType<Ok<ConsolidadoDiario[]>>(result.Result);
        Assert.NotNull(result);
        var okResult = (Ok<ConsolidadoDiario[]>)result.Result;
        Assert.Equal((consolidadosList.Count-1), okResult.Value!.Length);
    }

    [Fact]
    public async Task GetConsolidadosDiarios_Codigo_Comerciante_Invalido_Retorna_Validation_Problem()
    {
        // Arrange
        var ctx = new Mock<FluxoCaixaDbContext>();
        
        GetConsolidadosDiariosFilter filter = new()
        {
            ComercianteId = 0,
            DataInicial = DateOnly.Parse("2025-03-25"),
            DataFinal = null
        }; 
        
        //Act
        var result = await GetConsolidadosDiarios.GetAsync(filter, ctx.Object);
        
        //Assert
        Assert.IsType<ValidationProblem>(result.Result);
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task GetConsolidadosDiarios_Data_Invalida_Retorna_Validation_Problem()
    {
        // Arrange
        var ctx = new Mock<FluxoCaixaDbContext>();
        
        GetConsolidadosDiariosFilter filter = new()
        {
            ComercianteId = 0,
            DataInicial = default,
            DataFinal = null
        }; 
        
        //Act
        var result = await GetConsolidadosDiarios.GetAsync(filter, ctx.Object);
        
        //Assert
        Assert.IsType<ValidationProblem>(result.Result);
        Assert.NotNull(result);
    }
}