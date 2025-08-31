using Entities;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Repository;
using RepositoryContracts;
using ServiceContracts;
using Services;
using Serilog;
using CRUDExample.Filters.ActionFilters;

var builder = WebApplication.CreateBuilder(args);

// Logging

//builder.Host.ConfigureLogging(loggingProvider =>
//{
//    loggingProvider.ClearProviders();
//    loggingProvider.AddConsole();
//    loggingProvider.AddDebug();
//    loggingProvider.AddEventLog();
//});

// Serilog 
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services,LoggerConfiguration loggerConfiguration) =>
{
     loggerConfiguration.ReadFrom.Configuration(context.Configuration).ReadFrom.Services(services);

});

builder.Services.AddControllersWithViews(options=>
{
    // Global Filters
    //options.Filters.Add<ResponseHeaderActionFilter>(); //No parameter constructor

    //options.Filters.Add<ResponseHeaderActionFilter>(1); //  Order = 1 but no parameter constructor


    var _logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>(); // To resolve ILogger in ResponseHeaderActionFilter constructor

    options.Filters.Add(new ResponseHeaderActionFilter(_logger, "My-Key-Global", "My Value",2)); // With parameter constructor
});




builder.Services.AddRouting();

builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonsService, PersonsService>();

builder.Services.AddScoped<IPersonsRepository, PersonsRepository>();
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();

if(builder.Environment.IsEnvironment("Test") == false)
{
    builder.Services.AddDbContext<ApplicationDbContext>(
        options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        }
    );
}


//Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PersonsDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties;
    logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
});


var app = builder.Build();
app.UseSerilogRequestLogging();


if(builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpLogging();

//app.Logger.LogTrace("This is a trace message");
//app.Logger.LogDebug("This is a debug message");
//app.Logger.LogInformation("Application started");
//app.Logger.LogWarning("This is a warning message");
//app.Logger.LogError("This is an error message");
//app.Logger.LogCritical("This is a critical message");


if (builder.Environment.IsEnvironment("Test") == false)
{

    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
}

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();

public partial class Program { } // To use application programetically in integration tests