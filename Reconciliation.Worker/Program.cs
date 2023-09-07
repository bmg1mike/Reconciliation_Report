using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Reconciliation.Worker;
using ReconciliationReport.Data;
using ReconciliationReport.Services.Implementation;
using ReconciliationReport.Services.Interface;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((config, services) =>
    {
        services.AddHostedService<Worker>();
        services.AddDbContext<ReconciliationContext>(x => x.UseNpgsql(config.Configuration.GetConnectionString("DbConnection")));
        services.AddDbContext<NipContext>(x => x.UseSqlServer(config.Configuration.GetConnectionString("NipConnection")));
        services.AddHttpClient();
        services.AddScoped<IReconciliationService, ReconciliationService>();
    })
    .Build();

host.Run();
