using PropertyApp.Models;

namespace PropertyApp.Elasticsearch.Services;


public interface IElasticsearchService
{
    Task IndexPropertyAsync(Property property);
    Task BulkIndexPropertiesAsync(IEnumerable<Property> properties);
}
