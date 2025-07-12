using Nest;

namespace PropertyAnalyzer.Elasticsearch;

public static class ElasticsearchClientFactory
{
    public static ElasticClient CreateClient(IConfiguration config)
    {
        var uri = config["Elasticsearch:Uri"] ?? throw new InvalidOperationException("Elasticsearch:Uri configuration is required");
        var settings = new ConnectionSettings(new Uri(uri))
            .DefaultIndex("properties");

        return new ElasticClient(settings);
    }
}
