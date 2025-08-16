using ConfigurationExample.OptionModels;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.Configure<WeatherApiOptions>(builder.Configuration.GetSection("weatherapi"));

// Load my own config : Highet Priority
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile("MyOwnConfig.json", optional: true, reloadOnChange: true);
});

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

//app.UseEndpoints(endpoints =>
//{
//    endpoints.Map("/configf", async context =>
//    {
//        await context.Response.WriteAsync(app.Configuration["MyKey"]+"\n");

//        await context.Response.WriteAsync(app.Configuration.GetValue<string>("MyKey")+"\n");

//        await context.Response.WriteAsync(app.Configuration.GetValue<int>("x", 10).ToString()+"\n");

//    });
//});

app.MapControllers();



app.Run();
