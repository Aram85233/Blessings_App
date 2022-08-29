using Blessings;
using Blessings.Common.AspNet.Extensions;
using Blessings.Services.Options;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

configuration.SetBasePath(builder.Environment.ContentRootPath)
             .AddJsonFile("appsettings.local.json", optional: true)
             .AddEnvironmentVariables();

builder.Services.AddHealthChecks()
    .AddSqlServer(configuration.GetSection(DatabaseOptions.Section).Get<DatabaseOptions>().ConnectionString);

builder.Services.AddBlessingsLayer(configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllersWithViews().AddControllersAsServices();

var app = builder.Build();

app.AddAuthenticationConfiguration();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseHangfireDashboard();

app.Services.CollectOrders(app.Configuration);

app.Run();
