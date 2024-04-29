using System;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using AcmeCorpApi.Repository;
using AcmeCorpApi.Middleware;

namespace AcmeCorpApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkNpgsql()
                .AddDbContext<CustomersDbContext>(options => options.UseNpgsql(Configuration["Data:DbContext:CustomersConnectionString"]));
            services.AddEntityFrameworkNpgsql()
                .AddDbContext<OrdersDbContext>(options => options.UseNpgsql(Configuration["Data:DbContext:OrdersConnectionString"]));
            services.AddEntityFrameworkNpgsql()
                .AddDbContext<ProductsDbContext>(options => options.UseNpgsql(Configuration["Data:DbContext:ProductsConnectionString"]));

            services.AddControllersWithViews();

            services.AddScoped<ICustomersRepository, CustomersRepository>();
            services.AddScoped<IOrdersRepository, OrdersRepository>();
            services.AddScoped<IProductsRepository, ProductsRepository>();

            services.AddTransient<CustomersDbSeeder>();
            services.AddTransient<OrdersDbSeeder>();
            services.AddTransient<ProductsDbSeeder>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "AcmeCorp API",
                    Description = "AcmeCorp API Documentation",
                    Contact = new OpenApiContact { Name = "Jason Drawdy" },
                    License = new OpenApiLicense { Name = "MIT", Url = new Uri("https://en.wikipedia.org/wiki/MIT_License") }
                });
            });

            services.AddCors(o => o.AddPolicy("AllowAllPolicy", options =>
            {
                options.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
            }));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, 
        CustomersDbSeeder customersDbSeeder, OrdersDbSeeder ordersDbSeeder,
        ProductsDbSeeder productsDbSeeder)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            
            app.UseMiddleware<ApiKeyMiddleware>(); // For development this can be commented out, if desired.

            app.UseCors("AllowAllPolicy");
            app.UseSwagger();
            app.UseSwaggerUI(c => // http://localhost:5000/swagger
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}");
            });

            customersDbSeeder.SeedAsync(app.ApplicationServices).Wait();
            ordersDbSeeder.SeedAsync(app.ApplicationServices).Wait();
            productsDbSeeder.SeedAsync(app.ApplicationServices).Wait();
        }
    }
}
