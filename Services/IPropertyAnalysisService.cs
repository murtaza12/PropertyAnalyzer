using PropertyApp.DTOs;

namespace PropertyApp.Services;

public interface IPropertyAnalysisService
{
    Task<List<AnalysisResultDto>> AnalyzeAllAsync();
}
