using ModelValidation.CustomModelBinders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting();
builder.Services.AddControllers(options =>
{
    //options.ModelBinderProviders.Insert(0, new CustomBinderProvider()); //Testing collection binding
});
builder.Services.AddControllers().AddXmlSerializerFormatters();// Support XML serialization
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();


app.Run();
 