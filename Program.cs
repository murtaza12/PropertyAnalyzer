using Microsoft.EntityFrameworkCore;
using PropertyAnalyzer.Data.Seed;
using PropertyApp.Background;
using PropertyApp.Data;
using PropertyApp.Elasticsearch.Services;
using PropertyApp.Repositories;
using PropertyApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<IPropertyAnalysisService, PropertyAnalysisService>();
builder.Services.AddHostedService<PropertyAnalysisWorker>();

builder.Services.AddScoped<IElasticsearchService, ElasticsearchService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddScoped<SyncService>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var es = scope.ServiceProvider.GetRequiredService<IElasticsearchService>();
    await DbSeeder.SeedAsync(db, es);
}

app.UseSwagger();
app.UseSwaggerUI();
//app.UseAuthorization();
app.MapControllers();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
