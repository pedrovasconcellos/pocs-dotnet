using System;
using System.Globalization;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TestSQLite.Repository.SQLite;

namespace testMediator
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.ProjectName = Assembly.GetAssembly(typeof(Startup)).GetName().Name ?? string.Empty;
        }

        public IConfiguration Configuration { get; }

        public string ProjectName { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            CultureInfo.CurrentCulture = new CultureInfo("pt-br");
            
            services.AddControllers();
            services.AddMvc();
            services.AddApiVersioning(p =>
            {
                p.ApiVersionReader = new HeaderApiVersionReader(new string[] { "api-version" });
            });

            services.AddDefaultConfigureServiceSQLite("TestWebAPI");

            #region Swagger
            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = this.ProjectName,
                    Description = $"Example Software: {this.ProjectName}",
                });
            });
            #endregion Swagger
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider)
        {
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", $"Documentation v1: {this.ProjectName}");
            });
            #endregion

            SQLiteConfiguration.UpdateDatabase(provider.GetService<IServiceScopeFactory>());
        }
    }
}
