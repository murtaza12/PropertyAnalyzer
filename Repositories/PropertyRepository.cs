using Microsoft.EntityFrameworkCore;
using PropertyApp.Data;
using PropertyApp.Models;

namespace PropertyApp.Repositories;


public class PropertyRepository : IPropertyRepository
{
    private readonly ApplicationDbContext _context;

    public PropertyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Property>> GetAllAsync() =>
        await _context.Properties.AsNoTracking().ToListAsync();

    public async Task<List<Property>> GetBatchAsync(int skip, int take) =>
        await _context.Properties.AsNoTracking()
                                 .OrderBy(p => p.Id)
                                 .Skip(skip)
                                 .Take(take)
                                 .ToListAsync();

    public async Task<int> CountAsync() =>
        await _context.Properties.CountAsync();
}
