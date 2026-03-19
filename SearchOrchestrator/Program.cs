using Microsoft.EntityFrameworkCore;
using SearchOrchestrator.Application.Interfaces;
using SearchOrchestrator.Application.Services;
using SearchOrchestrator.Infrastructure.Persistence;
using SearchOrchestrator.Infrastructure.Search;
using SearchOrchestrator.Infrastructure.Workers;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("orchestrator-db"));

// TODO вынести DI
builder.Services.AddScoped<IIndexingService, IndexingService>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddSingleton<ISearchEngineClient, FakeSearchEngineClient>();

builder.Services.AddHostedService<IndexingWorker>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
