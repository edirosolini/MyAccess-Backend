// <copyright file="Injection.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAccess.DependencyInjection
{
    using System.Text;
    using AutoMapper;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using MyAccess.Domains.Providers;
    using MyAccess.Domains.Services;
    using MyAccess.Providers;
    using MyAccess.Services;

    public static class Injection
    {
        public static IServiceCollection InjectionStart(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MyDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddSingleton<IUserDao, UserDao>();
            services.AddSingleton<IMyDBDao, MyDBDao>();

            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IMyDBService, MyDBService>();
            services.AddSingleton<ILanguageService, LanguageService>();

            services.AddSingleton(new MapperConfiguration(m =>
            {
                m.AddProfile(new MappingProfile());
            }).CreateMapper());

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration.GetSection("SecurityToken:Issuer")?.Value,
                        ValidAudience = configuration.GetSection("SecurityToken:Audience")?.Value,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("SecurityToken:IssuerSigningKey")?.Value)),
                    };
                });

            var securitySchema = new OpenApiSecurityScheme
            {
                Description = "Using the Authorization header with the Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                },
            };

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", securitySchema);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } },
                });

                c.SwaggerDoc($"v1", new OpenApiInfo { Title = configuration.GetSection("ApplicationName")?.Value, Version = $"v1" });
            });

            return services;
        }

        public static void UpdateDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<MyDbContext>();
            context.Database.Migrate();
            context.Database.CloseConnection();
        }
    }
}
