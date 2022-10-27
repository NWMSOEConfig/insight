using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using InsightApi.Services;
using InsightApi.Models;

namespace InsightApi.Controllers;

[ApiController]
[Route("api/databaseTenants")]
public class TenantController : ControllerBase {
     private readonly DatabaseTenantService _tenantService;
     public TenantController(DatabaseTenantService tenantService) =>
        _tenantService = tenantService;
    
    [HttpGet]
    public async Task<List<Tenant>> Get() =>
        await _tenantService.GetAsync();
    
    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Tenant>> Get(string id)
    {
        var Tenant = await _tenantService.GetAsync(id);

        if (Tenant is null)
        {
            return NotFound();
        }

        return Tenant;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Tenant newTenant)
    {
        await _tenantService.CreateAsync(newTenant);

        return CreatedAtAction(nameof(Get), new { id = newTenant.Id }, newTenant);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Tenant updatedTenant)
    {
        var Tenant = await _tenantService.GetAsync(id);

        if(Tenant is null)
        {
            return NotFound();
        }
        updatedTenant.Id = Tenant.Id;

        await _tenantService.UpdateAsync(id, updatedTenant);

        return NoContent();
    }
    
    

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var Tenant = await _tenantService.GetAsync(id);

        if (Tenant is null)
        {
            return NotFound();
        }

        await _tenantService.RemoveAsync(id);

        return NoContent();
    }
}




