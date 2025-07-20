using Microsoft.Extensions.FileProviders;

// Static FIle
var builder = WebApplication.CreateBuilder(
    new WebApplicationOptions()
    {
        WebRootPath = "MyRoot"
    }
    );
var app = builder.Build();

app.UseStaticFiles(); // Uses MyRoot as the web root or wwwroot if not specified

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(
               builder.Environment.ContentRootPath, "MyWebRoot"
               ))
});



app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Welcome to the Static Files Example!");
    });
});

app.Run();
