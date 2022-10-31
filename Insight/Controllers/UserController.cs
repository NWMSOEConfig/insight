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




