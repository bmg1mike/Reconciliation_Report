using Infrastructure;
using Microsoft.EntityFrameworkCore;
using ReconciliationReport.Data;
using ReconciliationReport.Services.Implementation;
using ReconciliationReport.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ReconciliationContext>(x => x.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")));
builder.Services.AddDbContext<NipContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("NipConnection")));

builder.Services.AddScoped<IReconciliationService, ReconciliationService>();
builder.Services.AddHttpClient();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<ReconciliationContext>();
    await context.Database.MigrateAsync();
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");
}

app.Run();
