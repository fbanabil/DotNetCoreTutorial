using Microsoft.Extensions.Primitives;
using System.IO;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//app.Run(async (HttpContext context) =>
//{
    //context.Response.ContentType = "text/html";

    //context.Response.Headers["server"] = "My server";

    //if (context.Request.Method != "POST")
    //{
    //    context.Response.StatusCode = 405;
    //    await context.Response.WriteAsync("Only POST method is allowed.");
    //    //return;
    //}

    //await context.Response.WriteAsync("<h1>Welcome to my server</h1>");

    //string path = context.Request.Path;
    //await context.Response.WriteAsync($"<p>{path}</p>");



    //System.IO.StreamReader reader = new StreamReader(context.Request.Body);
    //string requestBody = await reader.ReadToEndAsync();

     

   
    //Dictionary<string, StringValues> queryDict =
    //Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(requestBody);

    //if(queryDict.ContainsKey("firstName"))
    //{
    //    string firstName = queryDict["firstName"][0];
    //    await context.Response.WriteAsync(firstName);
    //}

//});

//HTTP Details Completed like - Methods, status, Headers etc for request and response







app.Run();
