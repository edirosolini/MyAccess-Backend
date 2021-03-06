// <copyright file="Startup.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAccess.WebAPP
{
    using System.Reflection;
    using System.Text.Json.Serialization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using MyAccess.DependencyInjection;
    using MyAccess.WebAPP.Helpers;

    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddCors(c =>
            {
                c.AddPolicy(
                    $"EnableAllCors",
                    options =>
                    options
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin());
            });

            services.AddSwagger(this.Configuration);

            services.AddControllers()
                .AddNewtonsoftJson()
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            services.InjectionStart(this.Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsProduction())
            {
                app.UseHsts();
            }

            app.UseErrorHandlerHelper(env);

            app.UseStatusCodePages(async context =>
            {
                if (context.HttpContext.Response.StatusCode == 401)
                {
                    var problem = new ProblemDetails
                    {
                        Status = 401,
                        Title = "Token Validation Has Failed. Request Access Denied.",
                    };
                    await context.HttpContext.Response.WriteAsJsonAsync(problem).ConfigureAwait(false);
                }
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint($"v1/swagger.json", $"API Docs"));

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();
            app.UseRequestLocalization();
            app.UseCors($"EnableAllCors");

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseResponseCaching();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapRazorPages();

                endpoints.MapGet("/api/Version", async context =>
                {
                    await context.Response.WriteAsJsonAsync($"{Assembly.GetEntryAssembly().GetName().Version}");
                });
            });

            app.UpdateDatabase();
        }
    }
}
