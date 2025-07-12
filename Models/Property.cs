namespace PropertyApp.Models;

public class Property
{
    public int Id { get; set; }
    public string? Address { get; set; }
    public decimal Price { get; set; }
    public double AreaInSqFt { get; set; }
    public string? City { get; set; }
    public DateTime ListedDate { get; set; }

    // NEW: Geo-location (longitude, latitude)
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
