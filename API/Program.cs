using API.Extensions;
using API.Helpers;
using API.Middleware;
using AutoMapper;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddApplicationServices();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Configuration.AddJsonFile("appsettings.Development.json");
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(x => x.UseSqlite(connectionString));

//Middleware
var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();
// Redirect to errors controller when no controller found
app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseHttpsRedirection();
//to use static files like images folder
app.UseStaticFiles();
app.UseAuthorization();
app.UseSwaggerDocumentation();
// Route requests to the WeatherForecastController
app.MapControllers();
// to execute migration on start up
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = services.GetRequiredService<DataContext>();
        await context.Database.MigrateAsync();
        await DataContextSeed.SeedAsync(context,loggerFactory);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "Error occurred during migration");
    }
}
app.Run();

