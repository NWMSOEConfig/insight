using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using InsightApi.Models;
using Microsoft.EntityFrameworkCore;

public class Startup
{
    public IConfiguration configRoot
    {
        get;
    }

    public Startup(IConfiguration configuration)
    {
        configRoot = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors();
        services.AddControllers();
        services.AddDbContext<CategoryContext>(opt => opt.UseInMemoryDatabase("Category"));
        services.AddDbContext<SubcategoryContext>(opt => opt.UseInMemoryDatabase("Subcategory"));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();

        app.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true) // allow any origin // TODO: change this in deployment
            .AllowCredentials()); // allow credentials

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(x => x.MapControllers());

        // app.MapControllers();

    }
}