using RoutingExample.Constraints;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("months",typeof(MonthsCustomConstraints));
});


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
        if(context.Request.RouteValues.ContainsKey("id"))
        {
            int id = Convert.ToInt32(context.Request.RouteValues["id"]);
            await context.Response.WriteAsync($"Product ID: {id}\n");
        }
        else
        {
            await context.Response.WriteAsync("Product ID not provided.\n");
        }
    });

    //Route constraints
    endpoints.Map("products/{id:int?}", async context =>
    {
        int id = Convert.ToInt32(context.Request.RouteValues["id"]);
        await context.Response.WriteAsync($"Product ID (int): {id}\n");
    });

    // Route constraints with date parameter
    endpoints.Map("products/date/{date:datetime}", async context =>
    {
        if (context.Request.RouteValues.ContainsKey("date"))
        {
            DateTime date = Convert.ToDateTime(context.Request.RouteValues["date"]);
            await context.Response.WriteAsync($"Product Date: {date.ToShortDateString()}\n");
        }
        else
        {
            await context.Response.WriteAsync("Product Date not provided.\n");
        }
    });

    // Guid id as route parameter
    endpoints.Map("products/guid/{id:guid}", async context =>
    {
        if (context.Request.RouteValues.ContainsKey("id"))
        {
            Guid id = Guid.Parse(Convert.ToString(context.Request.RouteValues["id"])!);
            await context.Response.WriteAsync($"Product GUID: {id}\n");
        }
        else
        {
            await context.Response.WriteAsync("Product GUID not provided.\n");
        }
    });

    // minimum length constraint
    endpoints.Map("products/minlength/{name:minlength(3):maxlength(10)=Clock}", async context =>
    {
        // minlength(3):maxlength(10) = length(4,7)
        string name = Convert.ToString(context.Request.RouteValues["name"])!;
        await context.Response.WriteAsync($"Product Name: {name}\n");
    });

    //min max in int constraint
    endpoints.Map("products/minmax/{id:int:min(1):max(100)=10}", async context =>
    {
        // min(1):max(100) = range(1,100)
        int id = Convert.ToInt32(context.Request.RouteValues["id"]);
        await context.Response.WriteAsync($"Product ID (Min 1, Max 100): {id}\n");
    });

    // Range in int constraint
    endpoints.Map("products/range/{id:int:range(1,100)}", async context =>
    {
        int id = Convert.ToInt32(context.Request.RouteValues["id"]);
        await context.Response.WriteAsync($"Product ID (Range 1-100): {id}\n");
    });

    // Only alphabate
    endpoints.Map("products/alpha/{name:alpha=clock}", async context =>
    {
        string name = Convert.ToString(context.Request.RouteValues["name"])!;
        await context.Response.WriteAsync($"Product Name (Alpha): {name}\n");
    });

    // Regex (Regular Expression) as route constraint
    endpoints.Map("sales-report/{year:int:min(1900)}/" +
        "{month:regex(^(apr|jul|oct|jan)$)}", async context =>
    {
        int year=Convert.ToInt32(context.Request.RouteValues["year"]);
        string month = Convert.ToString(context.Request.RouteValues["month"])!;
        await context.Response.WriteAsync($"Sales Report for {month}-{year}\n");
    });

    // Use of custom constraint using matching : No benefit if it used one or very low number of time
    endpoints.Map("sales-report-check/{year:int:min(1900)}/" +
        "{month:months}", async context =>
        {
            int year = Convert.ToInt32(context.Request.RouteValues["year"]);
            string month = Convert.ToString(context.Request.RouteValues["month"])!;
            await context.Response.WriteAsync($"Sales Report for {month}-{year}\n");
        });


    // Routing Precedence : The more specific route should be defined first


});








app.Run(async context =>
{
    string? filename = Convert.ToString(context.Request.RouteValues["filename"]); ;
    await context.Response.WriteAsync($"File Name : {filename}");
    await context.Response.WriteAsync($"Request received at {context.Request.Path}");
});

app.Run();
