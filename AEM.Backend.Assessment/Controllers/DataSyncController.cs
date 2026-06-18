using AEM.Backend.Assessment.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using AEM.Backend.Assessment.Models;

namespace AEM.Backend.Assessment.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DataSyncController : ControllerBase
{
    private readonly ApiService _apiService;

    public DataSyncController(ApiService apiService)
    {
        _apiService = apiService;
    }

    [HttpGet("login")]
    public async Task<IActionResult> Login()
    {
        var result = await _apiService.LoginAsync();
        return Ok(result);
    }

    [HttpGet("preview-platform-well")]
    public async Task<IActionResult> PreviewPlatformWell()
    {
        var token = (await _apiService.LoginAsync()).Replace("\"", "");

        var data = await _apiService.GetPlatformWellActualAsync(token);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var platforms = JsonSerializer.Deserialize<List<Platform>>(data, options);

        return Ok(platforms);
    }

    [HttpPost("sync-platform-well")]
    public async Task<IActionResult> SyncPlatformWell()
    {
        var token = (await _apiService.LoginAsync()).Replace("\"", "");

        await _apiService.SyncPlatformWellAsync(token);

        return Ok("Sync completed successfully");
    }
}