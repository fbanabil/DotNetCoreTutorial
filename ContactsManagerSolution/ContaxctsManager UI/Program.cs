using Serilog;
using ContactsManager.UI;
using Middleware;

var builder = WebApplication.CreateBuilder(args);


// Serilog 
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services,LoggerConfiguration loggerConfiguration) =>
{
     loggerConfiguration.ReadFrom.Configuration(context.Configuration).ReadFrom.Services(services);

});


builder.Services.ConfigureServices(builder);


var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseExceptionHandlingMiddleware();
}


app.UseSerilogRequestLogging();



app.UseHttpLogging();



if (builder.Environment.IsEnvironment("Test") == false)
{

    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
}


app.UseStaticFiles();


app.UseRouting(); // Map the incoming request to respective controller action method

app.UseAuthentication(); // Reading Identity cookie and setting HttpContext.User

app.UseAuthorization();

app.MapControllers(); // Map the controller action methods to respective routes

app.Run();

public partial class Program { } 