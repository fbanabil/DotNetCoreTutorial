using Filters.ActionFilters;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repository;
using RepositoryContracts;
using ServiceContracts;
using Services;
using ContactsManager.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactsManager.UI
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
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
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


            // Enable Identity in this project
            services.AddIdentity<ApplicationUser,ApplicationRole>(options=>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                //options.SignIn.RequireConfirmedEmail = false;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddUserStore<UserStore<ApplicationUser,ApplicationRole,ApplicationDbContext,Guid>>()                
                .AddRoleStore<RoleStore<ApplicationRole,ApplicationDbContext,Guid>>();




            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder().
                RequireAuthenticatedUser().Build();
                //options.AddPolicy("AllowAnonymous", policy => policy.RequireAssertion(context => true));
                options.AddPolicy("NotAuthenticated", policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        return context.User?.Identity == null || context.User?.Identity?.IsAuthenticated == false;
                    });
                });

            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
            });



            services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties;
                logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
            });

        }
    }
}
