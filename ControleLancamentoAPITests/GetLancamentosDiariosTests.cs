using ControleLancamentoAPI.Endpoints.LancamentosDiarios.V1;
using FluxoCaixaContext;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Moq.EntityFrameworkCore;

namespace ControleLancamentoAPITests;

public class GetLancamentosDiariosTests
{
    [Fact]
    public async Task GetLancamentosDiarios_Com_Sucesso_Retorna_Lista()
    {
        // Arrange
        var ctx = new Mock<FluxoCaixaDbContext>();
        var list = new List<LancamentoDiario>()
        {
            new() {
                Id = Guid.NewGuid(),
                ComercianteId = 12,
                Data = DateOnly.Parse("2025-03-21"),
                Descricao = "Descricao",
                DataVencimento =  DateOnly.FromDateTime(DateTime.Today),
                Tipo =  TipoLancamentoEnum.Credito,
                Valor =  100M,
            },
            new() {
                Id = Guid.NewGuid(),
                ComercianteId = 12,
                Data = DateOnly.Parse("2025-03-22"),
                Descricao = "Descricao",
                DataVencimento =  DateOnly.FromDateTime(DateTime.Today),
                Tipo =  TipoLancamentoEnum.Debito,
                Valor =  50.78M,
            },
            new() {
                Id = Guid.NewGuid(),
                ComercianteId = 12,
                Data = DateOnly.Parse("2025-03-23"),
                Descricao = "Descricao",
                DataVencimento =  DateOnly.FromDateTime(DateTime.Today),
                Tipo =  TipoLancamentoEnum.Credito,
                Valor =  222M,
            },
            new() {
                Id = Guid.NewGuid(),
                ComercianteId = 12,
                Data = DateOnly.Parse("2025-03-24"),
                Descricao = "Descricao",
                DataVencimento =  DateOnly.FromDateTime(DateTime.Today),
                Tipo =  TipoLancamentoEnum.Debito,
                Valor =  340.89M,
            },
            new() {
                Id = Guid.NewGuid(),
                ComercianteId = 12,
                Data = DateOnly.Parse("2025-03-25"),
                Descricao = "Descricao",
                DataVencimento =  DateOnly.FromDateTime(DateTime.Today),
                Tipo =  TipoLancamentoEnum.Credito,
                Valor =  912.69M,
            },
            new() {
                Id = Guid.NewGuid(),
                ComercianteId = 11,
                Data = DateOnly.Parse("2025-03-26"),
                Descricao = "Descricao",
                DataVencimento =  DateOnly.FromDateTime(DateTime.Today.AddDays(2)),
                Tipo =  TipoLancamentoEnum.Credito,
                Valor =  100M,
            },
        };
        ctx.Setup(x => x.LancamentosDiarios).ReturnsDbSet(list);

        GetLancamentosDiariosFilter filter = new()
        {
            ComercianteId = 12,
            DataInicial = DateOnly.Parse("2025-03-21"),
            DataFinal = DateOnly.Parse("2025-03-26")
        }; 
        
        //Act
        var result = await GetLancamentosDiarios.GetAsync(filter, ctx.Object);
        
        //Assert
        Assert.IsType<Ok<LancamentoDiario[]>>(result.Result);
        Assert.NotNull(result);
        var okResult = (Ok<LancamentoDiario[]>)result.Result;
        Assert.Equal((list.Count-1), okResult.Value!.Length);
    }

    [Fact]
    public async Task GetLancamentosDiarios_Codigo_Comerciante_Invalido_Retorna_Validation_Problem()
    {
        // Arrange
        var ctx = new Mock<FluxoCaixaDbContext>();
        
        GetLancamentosDiariosFilter filter = new()
        {
            ComercianteId = 0,
            DataInicial = DateOnly.Parse("2025-03-25"),
            DataFinal = null
        }; 
        
        //Act
        var result = await GetLancamentosDiarios.GetAsync(filter, ctx.Object);
        
        //Assert
        Assert.IsType<ValidationProblem>(result.Result);
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task GetLancamentosDiarios_Data_Invalida_Retorna_Validation_Problem()
    {
        // Arrange
        var ctx = new Mock<FluxoCaixaDbContext>();
        
        GetLancamentosDiariosFilter filter = new()
        {
            ComercianteId = 0,
            DataInicial = default,
            DataFinal = null
        }; 
        
        //Act
        var result = await GetLancamentosDiarios.GetAsync(filter, ctx.Object);
        
        //Assert
        Assert.IsType<ValidationProblem>(result.Result);
        Assert.NotNull(result);
    }
}