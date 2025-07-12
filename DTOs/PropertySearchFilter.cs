namespace PropertyApp.DTOs;

public class PropertySearchFilter
{
    public string? City { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public double? MinArea { get; set; }
    public double? MaxArea { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? RadiusKm { get; set; } = "10km";
}
