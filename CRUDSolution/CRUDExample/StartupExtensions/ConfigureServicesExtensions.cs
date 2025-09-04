using CRUDExample.Filters.ActionFilters;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repository;
using RepositoryContracts;
using ServiceContracts;
using Services;

namespace CRUDExample
{
    public static class ConfigureServicesExtensions
    {
        public static void ConfigureServices(this IServiceCollection services,WebApplicationBuilder? builder)
        {


            services.AddTransient<ResponseHeaderActionFilter>(); // Registering ResponseHeaderActionFilter for DI


            services.AddControllersWithViews(options =>
            { 
                var _logger = services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();

                options.Filters.Add(new ResponseHeaderActionFilter(_logger)
                {
                    _key = "X-Global-Key",
                    _value = "Global-Value",
                    Order = 2
                });
            });




            services.AddRouting();

            services.AddScoped<ICountriesService, CountriesService>();
            
            
            //services.AddScoped<IPersonsGetterService, PersonsGetterServiceWithFewExelFields>();

            services.AddScoped<IPersonsGetterService, PersonsGetterServiceChild>();
            services.AddScoped<PersonsGetterService, PersonsGetterService>();

            services.AddScoped<IPersonsAdderService, PersonsAdderService>();
            services.AddScoped<IPersonsDeleterService, PersonsDeleterService>();
            services.AddScoped<IPersonsUpdaterService, PersonsUpdaterService>();
            services.AddScoped<IPersonsSorterService, PersonsSorterService>();



            services.AddScoped<IPersonsRepository, PersonsRepository>();
            services.AddScoped<ICountriesRepository, CountriesRepository>();

            if (builder?.Environment.IsEnvironment("Test") == false)
            {
                services.AddDbContext<ApplicationDbContext>(
                    options =>
                    {
                        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                    }
                );
            }

            services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties;
                logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
            });

        }
    }
}
