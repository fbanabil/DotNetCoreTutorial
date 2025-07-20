var builder = WebApplication.CreateBuilder(args);

// Builder.services.AddTransient<HomeController>()   for eaach controller
// For all controller
builder.Services.AddControllers();

// Add Routing
builder.Services.AddRouting();


var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

//app.UseEndpoints(endpoints =>
//{
//    //endpoints.Map("url", ...);
//    endpoints.MapControllers();
//});

// Simply
app.MapControllers();

app.Run();
