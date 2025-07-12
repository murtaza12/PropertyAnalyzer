using Nest;
using PropertyAnalyzer.Elasticsearch;
using PropertyApp.Models;

namespace PropertyApp.Elasticsearch.Services;


public class ElasticsearchService : IElasticsearchService
{
    private readonly ElasticClient _client;

    public ElasticsearchService(IConfiguration config)
    {
        _client = ElasticsearchClientFactory.CreateClient(config);
    }

    public async Task IndexPropertyAsync(Property property)
    {
        var doc = new
        {
            property.Id,
            property.Address,
            property.City,
            property.Price,
            property.AreaInSqFt,
            property.ListedDate,
            location = new { lat = property.Latitude, lon = property.Longitude }
        };

        await _client.IndexAsync(new IndexRequest<object>(doc, "properties", property.Id));
    }

    public async Task BulkIndexPropertiesAsync(IEnumerable<Property> properties)
    {
        var bulk = new BulkDescriptor();
        foreach (var prop in properties)
            bulk.Index<Property>(op => op.Document(prop));

        await _client.BulkAsync(bulk);
    }

}
