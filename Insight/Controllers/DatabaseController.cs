using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using Insight.Services;
using Insight.Models;

namespace Insight.Controllers;

public class DataServer {
     private readonly DatabaseSettingsService _settingsService;
     public DataServer(DatabaseSettingsService settingsService) {
        _settingsService = settingsService;
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




