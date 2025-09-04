using Entities;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Repository;
using RepositoryContracts;
using ServiceContracts;
using Services;
using Serilog;
using CRUDExample.Filters.ActionFilters;
using CRUDExample;

var builder = WebApplication.CreateBuilder(args);


// Serilog 
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services,LoggerConfiguration loggerConfiguration) =>
{
     loggerConfiguration.ReadFrom.Configuration(context.Configuration).ReadFrom.Services(services);

});


builder.Services.ConfigureServices(builder);


var app = builder.Build();
app.UseSerilogRequestLogging();


if(builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpLogging();



if (builder.Environment.IsEnvironment("Test") == false)
{

    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
}


app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();

public partial class Program { } 