var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


// Getendpoint routing before UseRouting
//app.Use(async (context, next) =>
//{
//    Microsoft.AspNetCore.Http.Endpoint endpoint = context.GetEndpoint();
//    await next(context);
//});



//Enable routing
app.UseRouting();

// Getendpoint routing after UseRouting
//app.Use(async (context, next) =>
//{
//    Microsoft.AspNetCore.Http.Endpoint endpoint = context.GetEndpoint();
//    await next(context);
//});


//app.UseEndpoints(endpoints =>
//{

// add endpoints 
//    endpoints.MapGet("/", async context =>
//    {
//        await context.Response.WriteAsync("Hello World!");
//    });

//    // Short circuit middlewares
//    endpoints.Map("map1",async (context) =>
//    {
//        await context.Response.WriteAsync("Hello from map1!");
//    });

//    endpoints.Map("map2", async (context) =>
//    {
//        await context.Response.WriteAsync("Hello from map2!");
//    });

//    endpoints.Map("map2", async (context) =>
//    {
//        await context.Response.WriteAsync("Hello from map2!");
//    });
//});

//endpoints.MapGet("/", async context =>
//{
//    await context.Response.WriteAsync("Hello World!");
//});

// Short circuit middlewares
//endpoints.MapGet("map1", async (context) =>
//{
//    await context.Response.WriteAsync("Hello from map1!");
//});

//endpoints.MapPost("map2", async (context) =>
//{
//    await context.Response.WriteAsync("Hello from map2!");
//});

//});

// Routing Parameters & default values
app.UseEndpoints(endpoints =>
{
    endpoints.Map("files/{filename=Hello}/{extension=txt}",async context =>
    {
        string? filename = Convert.ToString(context.Request.RouteValues["filename"]);
        string? extension = Convert.ToString(context.Request.RouteValues["extension"]);
        await context.Response.WriteAsync($"File requested: {filename}-{extension}\n");
        await context.Response.WriteAsync("In Files");
    });

    // Optional Routing Parameters

    endpoints.Map("products/details/{id?}", async context =>
    {
        int id=Convert.ToInt32(context.Request.RouteValues["id"]);
        await context.Response.WriteAsync($"Product ID: {id}\n");
    });
});








app.Run(async context =>
{
    string? filename = Convert.ToString(context.Request.RouteValues["filename"]); ;
    await context.Response.WriteAsync($"File Name : {filename}");
    await context.Response.WriteAsync($"Request received at {context.Request.Path}");
});

app.Run();
