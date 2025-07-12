using System.Collections.Concurrent;
using PropertyApp.DTOs;
using PropertyApp.Models;
using PropertyApp.Repositories;

namespace PropertyApp.Services;


public class PropertyAnalysisService : IPropertyAnalysisService
{
    private readonly IPropertyRepository _repository;

    public PropertyAnalysisService(IPropertyRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<AnalysisResultDto>> AnalyzeAllAsync()
    {
        var total = await _repository.CountAsync();
        var pageSize = 5000;
        var results = new ConcurrentBag<AnalysisResultDto>();

        var tasks = new List<Task>();

        for (int i = 0; i < total; i += pageSize)
        {
            var skip = i;
            tasks.Add(Task.Run(async () =>
            {
                var batch = await _repository.GetBatchAsync(skip, pageSize);
                foreach (var prop in batch)
                {
                    var analysis = Analyze(prop);
                    results.Add(analysis);
                }
            }));
        }

        await Task.WhenAll(tasks);
        return results.ToList();
    }

    private AnalysisResultDto Analyze(Property prop)
    {
        var pricePerSqFt = prop.AreaInSqFt > 0 ? (double)(prop.Price / (decimal)prop.AreaInSqFt) : 0;
        return new AnalysisResultDto
        {
            PropertyId = prop.Id,
            PricePerSqFt = Math.Round(pricePerSqFt, 2),
            City = prop.City
        };
    }
}
