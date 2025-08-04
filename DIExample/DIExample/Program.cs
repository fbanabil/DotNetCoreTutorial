using Services;
using ServiceContracts;
using Autofac.Extensions.DependencyInjection;
using Autofac;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Services.AddControllersWithViews();

// Temoporary data (cache/inmemory) - Singleton Service
// Database - Scoped Service
// Transient Service - For short lived data like encryption keys, tokens, email etc.


//builder.Services.Add(new ServiceDescriptor(
//    typeof(ICitiesService),
//    typeof(CitiesService),
//    ServiceLifetime.Scoped
//));

// Or using the AddScoped method directly. There are also AddSingleton and AddTransient methods.
//builder.Services.AddScoped<ICitiesService, CitiesService>();


// Registering the service with Autofac
builder.Host.ConfigureContainer<ContainerBuilder>
    (containerBuilder=>
    {
        containerBuilder.RegisterType<CitiesService>()
            .As<ICitiesService>()
            .InstancePerLifetimeScope(); // This is equivalent to Scoped in ASP.NET Core
    }
    );

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();


app.Run();
 