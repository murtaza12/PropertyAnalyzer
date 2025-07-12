using Microsoft.AspNetCore.Mvc;
using PropertyApp.DTOs;
using PropertyApp.Elasticsearch.Services;
using PropertyApp.Services;

namespace PropertyApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertyAnalysisController : ControllerBase
{
    private readonly SyncService _sync;
    private readonly IPropertyAnalysisService _analysisService;
    private readonly IAnalyticsService _analytics;

    public PropertyAnalysisController(SyncService sync, IPropertyAnalysisService analysisService, IAnalyticsService analytics)
    {
        _sync = sync;
        _analysisService = analysisService;
        _analytics = analytics;
    }

    [HttpGet("analyze")]
    public async Task<IActionResult> AnalyzeProperties()
    {
        var results = await _analysisService.AnalyzeAllAsync();
        return Ok(results);
    }

    [HttpPost("sync")]
    public async Task<IActionResult> SyncToElasticsearch()
    {
        await _sync.SyncAllToElasticsearchAsync();
        return Ok("Synced to Elasticsearch");
    }

    [HttpGet("avg-price-by-city")]
    public async Task<IActionResult> AvgPrice()
    {
        var result = await _analytics.GetAveragePriceByCityAsync();
        return Ok(result);
    }

    [HttpGet("top-cities")]
    public async Task<IActionResult> TopCities()
    {
        var result = await _analytics.GetTopCitiesAsync(5);
        return Ok(result);
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] PropertySearchFilter filter)
    {
        var result = await _analytics.SearchAsync(filter);
        return Ok(result);
    }
}
