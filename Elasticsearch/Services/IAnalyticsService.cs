using PropertyApp.DTOs;
using PropertyApp.Models;

namespace PropertyApp.Elasticsearch.Services;


public interface IAnalyticsService
{
    Task<IEnumerable<object>> GetAveragePriceByCityAsync();
    Task<IEnumerable<object>> GetTopCitiesAsync(int topN);
    Task<IEnumerable<Property>> SearchAsync(PropertySearchFilter filter);
}
