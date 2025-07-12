using PropertyApp.Models;

namespace PropertyApp.Repositories;

public interface IPropertyRepository
{
    Task<List<Property>> GetAllAsync();
    Task<List<Property>> GetBatchAsync(int skip, int take);
    Task<int> CountAsync();
}
