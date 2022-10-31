using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using InsightApi.Services;
using InsightApi.Models;

namespace InsightApi.Controllers;

[ApiController]
[Route("api/databaseCommits")]
public class CommitController : ControllerBase {
     private readonly DatabaseCommitService _commitsService;
     public CommitController(DatabaseCommitService commitsService) =>
        _commitsService = commitsService;
    
    [HttpGet]
    public async Task<List<Commit>> Get() =>
        await _commitsService.GetAsync();
    
    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Commit>> Get(string id)
    {
        var commit = await _commitsService.GetAsync(id);

        if (commit is null)
        {
            return NotFound();
        }

        return commit;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Commit newCommit)
    {
        await _commitsService.CreateAsync(newCommit);

        return CreatedAtAction(nameof(Get), new { id = newCommit.Id }, newCommit);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Commit updatedCommit)
    {
        var commit = await _commitsService.GetAsync(id);

        if(commit is null)
        {
            return NotFound();
        }
        updatedCommit.Id = commit.Id;

        await _commitsService.UpdateAsync(id, updatedCommit);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var commit = await _commitsService.GetAsync(id);

        if (commit is null)
        {
            return NotFound();
        }

        await _commitsService.RemoveAsync(id);

        return NoContent();
    }
}



