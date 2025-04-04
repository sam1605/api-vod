using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using api_vod.Models;

namespace VodApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VersesController : ControllerBase
{
    private readonly HttpClient _client;
    private readonly SupabaseConfig _config;

    public VersesController(IOptions<SupabaseConfig> config)
    {
        _client = new HttpClient();
        _config = config.Value;

        if (string.IsNullOrEmpty(_config.Url) || string.IsNullOrEmpty(_config.Key))
        {
            throw new Exception("Supabase URL or Key is missing.");
        }

        // ✅ Set BaseAddress to avoid InvalidOperationException
        _client.BaseAddress = new Uri(_config.Url);
        _client.DefaultRequestHeaders.Add("apikey", _config.Key);
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_config.Key}");
    }

    // ✅ GET: Retrieve a random verse from vw_verses
    [HttpGet("random")]
    public async Task<IActionResult> GetRandomVerse()
    {
        try
        {
            var response = await _client.GetFromJsonAsync<List<VwVerse>>("/rest/v1/vw_random_verse?select=json");

            if (response == null || response.Count == 0)
            {
                return NotFound("No verses found.");
            }

            return Ok(response[0].Json);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // ✅ POST: Add a new verse
    [HttpPost]
    public async Task<IActionResult> AddVerse([FromBody] Verse verse)
    {
        if (verse == null || string.IsNullOrEmpty(verse.Reference) || string.IsNullOrEmpty(verse.VerseText))
        {
            return BadRequest("Invalid verse data.");
        }

        var response = await _client.PostAsJsonAsync($"/rest/v1/verses", verse);

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, "Failed to add the verse.");
        }

        return Created($"/api/verses/{verse.Id}", verse);
    }
}
