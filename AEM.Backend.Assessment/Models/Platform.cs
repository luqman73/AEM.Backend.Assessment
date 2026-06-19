using System.Text.Json.Serialization;

namespace AEM.Backend.Assessment.Models;

public class Platform
{
    public int Id { get; set; } // DB identity

    public int ExternalId { get; set; } // API id

    public string? UniqueName { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [JsonPropertyName("well")]
    public List<Well>? Well { get; set; }
}