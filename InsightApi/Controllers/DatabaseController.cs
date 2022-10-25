using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using InsightApi.Services;
using InsightApi.Models;

namespace InsightApi.Controllers;

[ApiController]
[Route("api/databaseSettings")]
public class DatabaseController : ControllerBase {
     private readonly DatabaseSettingsService _settingsService;
     public DatabaseController(DatabaseSettingsService settingsService) =>
        _settingsService = settingsService;
    
    [HttpGet]
    public async Task<List<Setting>> Get() =>
        await _settingsService.GetAsync();
    
    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Setting>> Get(string id)
    {
        var setting = await _settingsService.GetAsync(id);

        if (setting is null)
        {
            return NotFound();
        }

        return setting;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Setting newSetting)
    {
        await _settingsService.CreateAsync(newSetting);

        return CreatedAtAction(nameof(Get), new { id = newSetting.Id }, newSetting);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Setting updatedSetting)
    {
        var setting = await _settingsService.GetAsync(id);

        if(setting is null)
        {
            return NotFound();
        }
        updatedSetting.Id = setting.Id;

        await _settingsService.UpdateAsync(id, updatedSetting);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var setting = await _settingsService.GetAsync(id);

        if (setting is null)
        {
            return NotFound();
        }

        await _settingsService.RemoveAsync(id);

        return NoContent();
    }
}




