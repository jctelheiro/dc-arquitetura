// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using ConsolidadoDiarioServiceJob;
using FluxoCaixaContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using var loggerFactory = LoggerFactory.Create(static builder =>
{
    builder
        .AddFilter("Microsoft", LogLevel.Warning)
        .AddFilter("System", LogLevel.Warning)
        .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
        .AddConsole();
});

ILogger logger = loggerFactory.CreateLogger<Program>();
logger.LogInformation("Iniciando consolidado diário");
try
{
    var connectionString = Environment.GetEnvironmentVariable("FLUXOCAIXA_CONNECTION_STRING");
    var startEarlierDaysProcessing = Environment.GetEnvironmentVariable("START_EARLIER_DAYS_PROCESSING");

    ArgumentException.ThrowIfNullOrEmpty(connectionString);
    ArgumentException.ThrowIfNullOrEmpty(startEarlierDaysProcessing);

    DbContextOptionsBuilder<FluxoCaixaDbContext> builder = new();
    builder.UseNpgsql(connectionString);
    builder.UseNpgsql(a => a.EnableRetryOnFailure());

    int.TryParse(startEarlierDaysProcessing, out int daysProcessing);
    DateOnly startEarlierDateProcessing = DateOnly.FromDateTime(DateTime.Today.AddDays(-daysProcessing));

    logger.LogInformation($"Processando período entre: {startEarlierDateProcessing} até {DateOnly.FromDateTime(DateTime.Now.AddDays(-1))}");
    
    Stopwatch stopWatch = new Stopwatch();
    stopWatch.Start();

    using (var ctx = new FluxoCaixaDbContext(builder.Options))
    {
        await ConsolidadoDiarioService.ExecuteJobAsync(logger, ctx, startEarlierDateProcessing);
    }

    stopWatch.Stop();
    TimeSpan ts = stopWatch.Elapsed;

    // Format and display the TimeSpan value.
    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
        ts.Hours, ts.Minutes, ts.Seconds,
        ts.Milliseconds / 10);
    
    logger.LogInformation($"Tempo de processamento: {elapsedTime}");
}
catch (Exception ex)
{
    logger.LogError($"Ocorreu um erro: {ex.Message}");    
}

logger.LogInformation("Finalizando consolidado diario");