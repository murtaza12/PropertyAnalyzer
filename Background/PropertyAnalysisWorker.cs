using PropertyApp.Services;

namespace PropertyApp.Background;


public class PropertyAnalysisWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public PropertyAnalysisWorker(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IPropertyAnalysisService>();
            var results = await service.AnalyzeAllAsync();
            Console.WriteLine($"Analysis run at {DateTime.Now} — Records: {results.Count}");
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}
