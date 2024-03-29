
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
        app.UseStaticFiles();

    }
}