using System.Text;
using System.Text.Json;
using AEM.Backend.Assessment.Data;
using AEM.Backend.Assessment.Models;
using Microsoft.EntityFrameworkCore;

namespace AEM.Backend.Assessment.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly AppDbContext _db;

    public ApiService(HttpClient httpClient, AppDbContext db)
    {
        _httpClient = httpClient;
        _db = db;
    }

    public async Task<string> LoginAsync()
    {
        var loginRequest = new
        {
            username = "user@aemenersol.com",
            password = "Test@123"
        };

        var content = new StringContent(
            JsonSerializer.Serialize(loginRequest),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PostAsync(
            "http://test-demo.aemenersol.com/api/account/login",
            content);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetPlatformWellActualAsync(string token)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            "http://test-demo.aemenersol.com/api/PlatformWell/GetPlatformWellActual"
        );

        request.Headers.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    public async Task SyncPlatformWellAsync(string token)
    {
        var data = await GetPlatformWellActualAsync(token);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var platforms = JsonSerializer.Deserialize<List<Platform>>(data, options)
                        ?? new List<Platform>();

        foreach (var platform in platforms)
        {
            var existingPlatform = await _db.Platforms
                .FirstOrDefaultAsync(p => p.ExternalId == platform.Id);

            if (existingPlatform == null)
            {
                existingPlatform = new Platform
                {
                    ExternalId = platform.Id,
                    UniqueName = platform.UniqueName,
                    Latitude = platform.Latitude,
                    Longitude = platform.Longitude,
                    CreatedAt = platform.CreatedAt,
                    UpdatedAt = platform.UpdatedAt
                };

                _db.Platforms.Add(existingPlatform);
            }
            else
            {
                existingPlatform.UniqueName = platform.UniqueName;
                existingPlatform.Latitude = platform.Latitude;
                existingPlatform.Longitude = platform.Longitude;
                existingPlatform.UpdatedAt = platform.UpdatedAt;
            }
            
            // Save insert platform first so that well can refer to the platform
            await _db.SaveChangesAsync();

            // Handle Wells
            if (platform.Well != null)
            {
                foreach (var well in platform.Well)
                {
                    var existingWell = await _db.Wells
                        .FirstOrDefaultAsync(w => w.ExternalId == well.Id);

                    if (existingWell == null)
                    {
                        var newWell = new Well
                        {
                            ExternalId = well.Id,
                            UniqueName = well.UniqueName,
                            Latitude = well.Latitude,
                            Longitude = well.Longitude,
                            CreatedAt = well.CreatedAt,
                            UpdatedAt = well.UpdatedAt,

                            PlatformId = existingPlatform.Id
                        };

                        _db.Wells.Add(newWell);
                    }
                    else
                    {
                        existingWell.UniqueName = well.UniqueName;
                        existingWell.Latitude = well.Latitude;
                        existingWell.Longitude = well.Longitude;
                        existingWell.UpdatedAt = well.UpdatedAt;
                    }
                }
            }
        }

        await _db.SaveChangesAsync();
    }
}