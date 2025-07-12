using Microsoft.EntityFrameworkCore;
using PropertyApp.Data;

namespace PropertyApp.Elasticsearch.Services;


public class SyncService
{
    private readonly ApplicationDbContext _db;
    private readonly IElasticsearchService _es;

    public SyncService(ApplicationDbContext db, IElasticsearchService es)
    {
        _db = db;
        _es = es;
    }

    public async Task SyncAllToElasticsearchAsync()
    {
        var properties = await _db.Properties.ToListAsync();
        await _es.BulkIndexPropertiesAsync(properties);
    }
}
