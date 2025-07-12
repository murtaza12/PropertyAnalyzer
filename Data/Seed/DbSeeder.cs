using Microsoft.EntityFrameworkCore;
using PropertyApp.Data;
using PropertyApp.Elasticsearch.Services;
using PropertyApp.Models;

namespace PropertyAnalyzer.Data.Seed;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext db, IElasticsearchService es)
    {
        if (await db.Properties.AnyAsync()) return;

        var rnd = new Random();
        var cities = new[] { "Lahore", "Karachi", "Islamabad", "Peshawar", "Multan" };

        var props = Enumerable.Range(1, 1000).Select(i => new Property
        {
            Address = $"House {i}, Street {rnd.Next(1, 20)}",
            City = cities[rnd.Next(cities.Length)],
            Price = rnd.Next(3000000, 20000000),
            AreaInSqFt = rnd.Next(600, 1000),
            ListedDate = DateTime.UtcNow.AddDays(-rnd.Next(100)),
            Latitude = 31.5 + rnd.NextDouble(),
            Longitude = 74.3 + rnd.NextDouble()
        }).ToList();

        await db.Properties.AddRangeAsync(props);
        await db.SaveChangesAsync();

        await es.BulkIndexPropertiesAsync(props);
    }
}
