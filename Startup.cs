using CRUD_BAL.Service;
using CRUD_DAL.Data;
using CRUD_DAL.Interface;
using CRUD_DAL.Models;
using CRUD_DAL.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CRUDAspNetCore5WebAPI
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            // Register DbContext with SQL Server connection string
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));

            // Register controllers and services
            services.AddControllers();

            // Register HTTP Client
            services.AddHttpClient();

            // Register repository and service with scoped lifetime
            services.AddScoped<IRepository<Person>, RepositoryPerson>();
            services.AddScoped<PersonService>();

            // Set up Swagger with a custom version and title
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CRUDAspNetCore5WebAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // Use Developer exception page and Swagger UI
                app.UseDeveloperExceptionPage();

                // Enable Swagger and configure the Swagger UI endpoint
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CRUDAspNetCore5WebAPI v1");
                    c.RoutePrefix = "docs";  // Swagger UI will now be available at /docs
                });
            }

            // Enforce HTTPS redirection
            app.UseHttpsRedirection();

            // Set up routing for API endpoints
            app.UseRouting();

            // Enable authorization middleware
            app.UseAuthorization();

            // Map controller routes (ensures API actions are properly routed)
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();  // Maps API controller routes
            });
        }
    }
}
