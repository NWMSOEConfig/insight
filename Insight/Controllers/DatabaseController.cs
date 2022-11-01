using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using Insight.Services;
using Insight.Models;

namespace Insight.Controllers;

public class DataServer {
     private readonly DatabaseSettingsService _settingsService;
     private readonly DatabaseCommitService _commitService;
     private readonly DatabaseTenantService _tenantService;
     private readonly DatabaseQueuedChangeService _queuedChangeService;
     private readonly DatabaseUserService _userService;
     public DataServer(DatabaseSettingsService settingsService, DatabaseTenantService tenantService, DatabaseCommitService commitService,
        DatabaseQueuedChangeService databaseQueuedChangeService, DatabaseUserService userService) {
        _settingsService = settingsService;
        _commitService = commitService;
        _tenantService = tenantService;
        _userService = userService;
        _queuedChangeService = databaseQueuedChangeService;
    }

    public async Task<List<Setting>> GetEnvironmentSettingsAsync(string tenantName)
    {
        return await _settingsService.GetEnvironmentAsync(tenantName);
    }

    public async Task<List<Setting>> GetSettingsAsync()
    {
        return await _settingsService.GetAsync();
    }
    
}




