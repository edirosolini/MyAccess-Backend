// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAccess.WorkerService
{
    using System.Reflection;
    using MyAccess.DependencyInjection;
    using MyAccess.Services.Extensions;
    using MyAccess.WorkerService.CronJobs;
    using Serilog;

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseWindowsService(configure =>
            {
                configure.ServiceName = Assembly.GetExecutingAssembly().GetName().Name;
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.InjectionStart(hostContext.Configuration);

                services.AddCronJob<BackupDbJob>(c =>
                {
                    c.TimeZoneInfo = TimeZoneInfo.Local;
                    c.CronExpression = hostContext.Configuration.GetSection("CronExpression:BackupDBJob")?.Value;
                });
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Debug);
            })
            .UseSerilog((HostBuilderContext context, LoggerConfiguration loggerConfiguration) =>
            {
                loggerConfiguration.ReadFrom.Configuration(context.Configuration);
            });
    }
}
