using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using Insight.Services;
using Insight.Models;

namespace Insight.Controllers;

[ApiController]
[Route("api/database")]
public class UserController : ControllerBase {
    private DataServer _dbController;
     public UserController(DataServer databaseController) =>
        _dbController = databaseController;


    [HttpGet("{environment}")]
    public async Task<List<DatabaseSetting>> environmentContext(string environment) 
    {
        List<DatabaseSetting> vals = new List<DatabaseSetting>();

        vals = await _dbController.GetEnvironmentSettingsAsync(environment);

        return vals;
    }

    [HttpGet]
    public async Task<List<DatabaseSetting>> getDatabaseSettings()
    {
        List<DatabaseSetting> DatabaseSettings = new List<DatabaseSetting>();

        DatabaseSettings = await _dbController.GetSettingsAsync();

        return DatabaseSettings;
    }
        
}




