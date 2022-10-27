using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using InsightApi.Services;
using InsightApi.Models;

namespace InsightApi.Controllers;

[ApiController]
[Route("api/database")]
public class UserController : ControllerBase {
    private DataServer _dbController;
     public UserController(DataServer databaseController) =>
        _dbController = databaseController;

    [HttpGet("{environment:length(24)}")]
    public async Task<List<Setting>> environmentContext(string environment) 
    {
        List<Setting> vals = new List<Setting>();

        vals = await _dbController.GetEnvironmentSettingsAsync(environment);

        return vals;
    }

    [HttpGet]
    public async Task<List<Setting>> getSettings()
    {
        List<Setting> settings = new List<Setting>();

        settings = await _dbController.GetSettingsAsync();

        return settings;
    }
        
}




