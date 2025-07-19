using MiddlewareExample.CustomMiddleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<MyCustomMiddleware>();

var app = builder.Build();

//app.Run(async (HttpContext context) =>
//{
//    await context.Response.WriteAsync("Hello, World!");
//});

// app.Run() don't forward to subsecuent middleware 


//middleware 1
app.Use(async (HttpContext context, RequestDelegate next) =>
{
    await context.Response.WriteAsync("Hello, World!\n");
    await next(context); // Call the next middleware in the pipeline
});



// middleware 2
app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("Hello, World! Again\n");
    await next(context); // Call the next middleware in the pipeline
});
//Can or not use data type as it is defined that way. but if no datatypr next() must be used



//instead of middleware2
app.UseMiddleware<MyCustomMiddleware>();


//Cumtom built middleware in IapplicationBuilder
app.UseMyCustomMiddleware(); // Custom middleware extension method

// conventional middleware
app.UseHelloCustomMiddleware();


// middleware 3
app.Run(async (HttpContext context) =>
{
    await context.Response.WriteAsync("Hello, World! Again! and Again");
});



app.Run();
