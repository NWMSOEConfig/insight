using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using InsightApi.Services;
using InsightApi.Models;

namespace InsightApi.Controllers;

[ApiController]
[Route("api/databaseQueuedChanges")]
public class QueuedChangesController : ControllerBase {
     private readonly DatabaseQueuedChangeService _queuedChangesService;
     public QueuedChangesController(DatabaseQueuedChangeService queuedChangesService) =>
        _queuedChangesService = queuedChangesService;
    
    [HttpGet]
    public async Task<List<QueuedChange>> Get() =>
        await _queuedChangesService.GetAsync();
    
    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<QueuedChange>> Get(string id)
    {
        var QueuedChange = await _queuedChangesService.GetAsync(id);

        if (QueuedChange is null)
        {
            return NotFound();
        }

        return QueuedChange;
    }

    [HttpPost]
    public async Task<IActionResult> Post(QueuedChange newQueuedChange)
    {
        await _queuedChangesService.CreateAsync(newQueuedChange);

        return CreatedAtAction(nameof(Get), new { id = newQueuedChange.Id }, newQueuedChange);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, QueuedChange updatedQueuedChange)
    {
        var QueuedChange = await _queuedChangesService.GetAsync(id);

        if(QueuedChange is null)
        {
            return NotFound();
        }
        updatedQueuedChange.Id = QueuedChange.Id;

        await _queuedChangesService.UpdateAsync(id, updatedQueuedChange);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var QueuedChange = await _queuedChangesService.GetAsync(id);

        if (QueuedChange is null)
        {
            return NotFound();
        }

        await _queuedChangesService.RemoveAsync(id);

        return NoContent();
    }
}




