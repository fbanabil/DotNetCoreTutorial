var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// This middleware will run for username query parameter
app.UseWhen(
    context => context.Request.Query.ContainsKey("username"),
    app =>
    {
        app.Use(async (context, next) =>
        {
            await context.Response.WriteAsync("Hello from Middleware branch.\n");
            await next();
        });
    });

app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("Hello from Main Middleware.\n");
    await next(context);
});

app.Run();
