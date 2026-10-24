using CartService.Api.Grpc.V1;
using CartService.Application.DependencyInjection;
using CartService.Infrastructure.DependencyInjection;
using migApp.Shared.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseMigAppSerilog("CartService");

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

await app.MigrateDatabaseAsync();
await app.SeedDatabaseAsync();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.MapGrpcService<GrpcServer>();
app.MapGrpcHealthChecksService();

app.Run();