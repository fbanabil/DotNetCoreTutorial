var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
var app = builder.Build();

if (app.Environment.IsDevelopment() ||
    app.Environment.IsStaging() || 
    app.Environment.IsEnvironment("Beta"))
{
    // Enable detailed error pages in development
    app.UseDeveloperExceptionPage();
}




app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
