namespace AEM.Backend.Assessment.Models;

public class Well
{
    public int Id { get; set; } // DB identity

    public int ExternalId { get; set; } // API id

    public int PlatformId { get; set; }

    public string? UniqueName { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}