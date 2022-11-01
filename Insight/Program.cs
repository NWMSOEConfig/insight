using Insight.Models;
using Insight.Services;
using Insight.Controllers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<DBSettingConnection>(
    builder.Configuration.GetSection("DBSettingConnection"));

builder.Services.AddScoped<DataServer>();
builder.Services.AddSingleton<DatabaseSettingsService>();

builder.Services.Configure<DBSettingConnection>(
    builder.Configuration.GetSection("DBCommitConnection"));

builder.Services.AddSingleton<DatabaseCommitService>();

builder.Services.Configure<DBSettingConnection>(
    builder.Configuration.GetSection("DBUserConnection"));

builder.Services.AddSingleton<DatabaseUserService>();

builder.Services.Configure<DBSettingConnection>(
    builder.Configuration.GetSection("DBQueuedChangesConnection"));

builder.Services.AddSingleton<DatabaseQueuedChangeService>();

builder.Services.Configure<DBSettingConnection>(
    builder.Configuration.GetSection("DBTenantConnection"));

builder.Services.AddSingleton<DatabaseTenantService>();

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);
var app = builder.Build();
startup.Configure(app, builder.Environment);
app.Run();