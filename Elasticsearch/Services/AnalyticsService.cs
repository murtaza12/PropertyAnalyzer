using System.Text.Json;
using Nest;
using PropertyAnalyzer.Elasticsearch;
using PropertyApp.DTOs;
using PropertyApp.Models;

namespace PropertyApp.Elasticsearch.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly ElasticClient _client;

    public AnalyticsService(IConfiguration config)
    {
        _client = ElasticsearchClientFactory.CreateClient(config);
    }

    public async Task<IEnumerable<object>> GetAveragePriceByCityAsync()
    {
        var res = await _client.SearchAsync<Property>(s => s
            .Size(0)
            .Aggregations(a => a
                .Terms("cities", t => t
                    .Field(p => p.City.Suffix("keyword"))
                    .Size(100)
                    .Aggregations(aa => aa
                        .Average("avg_price", ap => ap.Field(p => p.Price))
                    )
                )
            )
        );

        return res.Aggregations.Terms("cities").Buckets.Select(b => new
        {
            City = b.Key,
            AvgPrice = b.Average("avg_price")?.Value
        });
    }

    public async Task<IEnumerable<object>> GetTopCitiesAsync(int topN)
    {
        var res = await _client.SearchAsync<Property>(s => s
            .Size(0)
            .Aggregations(a => a
                .Terms("top_cities", t => t
                    .Field(p => p.City.Suffix("keyword"))
                    .Size(topN)
                )
            )
        );

        return res.Aggregations.Terms("top_cities").Buckets.Select(b => new
        {
            City = b.Key,
            Count = b.DocCount
        });
    }

    public async Task<IEnumerable<Property>> SearchAsync(PropertySearchFilter filter)
{
    var must = new List<Func<QueryContainerDescriptor<dynamic>, QueryContainer>>();

    if (!string.IsNullOrEmpty(filter.City))
        must.Add(q => q.Match(m => m.Field("city").Query(filter.City)));

    if (filter.MinPrice != null || filter.MaxPrice != null)
        must.Add(q => q.Range(r => r.Field("price")
                                     .GreaterThanOrEquals((double?)filter.MinPrice)
                                     .LessThanOrEquals((double?)filter.MaxPrice)));

    if (filter.MinArea != null || filter.MaxArea != null)
        must.Add(q => q.Range(r => r.Field("area")
                                     .GreaterThanOrEquals((double?)filter.MinArea)
                                     .LessThanOrEquals((double?)filter.MaxArea)));

    if (filter.Latitude != null && filter.Longitude != null)
    {
        must.Add(q => q.GeoDistance(g => g
            .Field("location")
            .Distance(filter.RadiusKm)
            .Location(filter.Latitude.Value, filter.Longitude.Value)));
    }

    var response = await _client.SearchAsync<dynamic>(s => s
        .Index("properties")
        .Size(100)
        .Query(q => q.Bool(b => b.Must(must)))
    );

    return response.Hits.Select(h => JsonSerializer.Deserialize<Property>(h.Source.ToString())).Cast<Property>();
}

}
