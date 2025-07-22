var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();
//app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
