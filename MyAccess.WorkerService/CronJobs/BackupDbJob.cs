// <copyright file="BackupDbJob.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAccess.WorkerService.CronJobs
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using MyAccess.Domains.Configs;
    using MyAccess.Domains.Services;
    using MyAccess.Services;

    public class BackupDbJob : CronJobService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<BackupDbJob> logger;

        private readonly IServiceScope scope;
        private readonly IMyDBService service;

        public BackupDbJob(IScheduleConfig<BackupDbJob> config, IServiceScopeFactory scopeFactory, IConfiguration configuration, ILogger<BackupDbJob> logger)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            this.configuration = configuration;
            this.logger = logger;

            this.scope = scopeFactory.CreateScope();
            this.service = this.scope.ServiceProvider.GetService<IMyDBService>();
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("BackupDbJob: Task started.");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            try
            {
                this.service.Backup();
                this.logger.LogInformation("the backup db created.");
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "the backup db error.");
            }

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("BackupDbJob: Task stopped.");
            return base.StopAsync(cancellationToken);
        }
    }
}
