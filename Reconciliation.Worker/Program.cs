using Microsoft.EntityFrameworkCore;
using Reconciliation.Worker;
using ReconciliationReport.Data;
using ReconciliationReport.Services.Implementation;
using ReconciliationReport.Services.Interface;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((config,services) =>
    {
        services.AddHostedService<Worker>();
        services.AddDbContext<ReconciliationContext>(x => x.UseSqlServer(config.Configuration.GetConnectionString("DbConnection")));
        services.AddScoped<IReconciliationService,ReconciliationService>();
    })
    .Build();

host.Run();
