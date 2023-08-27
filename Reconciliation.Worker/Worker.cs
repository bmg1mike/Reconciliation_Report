using ReconciliationReport.Services.Interface;

namespace Reconciliation.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration configuration;
    private readonly IServiceProvider serviceProvider;

    public Worker(ILogger<Worker> logger, IConfiguration configuration, IServiceProvider serviceProvider)
    {
        _logger = logger;
        this.configuration = configuration;
        this.serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            using var scope = serviceProvider.CreateScope();
            var reconciliationService = scope.ServiceProvider.GetRequiredService<IReconciliationService>();
            if (configuration["ReportService"] == "Inward")
            {
               await reconciliationService.CompareInward();
            }
            else if (configuration["ReportService"] == "Outward")
            {
                await reconciliationService.CompareOutward();
            }
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }
}
