using InsightApi.Models;
using InsightApi.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<DBSettingConnection>(
    builder.Configuration.GetSection("DBSettingConnection"));

builder.Services.AddSingleton<DatabaseSettingsService>();

builder.Services.Configure<DBCommmitConnection>(
    builder.Configuration.GetSection("DBCommitConnection"));

builder.Services.AddSingleton<DatabaseCommitService>();

builder.Services.Configure<DBUserConnection>(
    builder.Configuration.GetSection("DBUserConnection"));

builder.Services.AddSingleton<DatabaseUserService>();

builder.Services.Configure<DBQueuedChangesConnection>(
    builder.Configuration.GetSection("DBQueuedChangesConnection"));

builder.Services.AddSingleton<DatabaseQueuedChangeService>();

builder.Services.Configure<DBTenantConnection>(
    builder.Configuration.GetSection("DBTenantConnection"));

builder.Services.AddSingleton<DatabaseTenantService>();

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);
var app = builder.Build();
startup.Configure(app, builder.Environment);
app.Run();