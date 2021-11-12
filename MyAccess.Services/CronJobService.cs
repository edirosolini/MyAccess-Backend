// <copyright file="CronJobService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAccess.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Cronos;
    using Microsoft.Extensions.Hosting;

    public abstract class CronJobService : IHostedService, IDisposable
    {
        private readonly CronExpression expression;
        private readonly TimeZoneInfo timeZoneInfo;
        private System.Timers.Timer timer;

        protected CronJobService(string cronExpression, TimeZoneInfo timeZoneInfo)
        {
            this.expression = CronExpression.Parse(cronExpression);
            this.timeZoneInfo = timeZoneInfo;
        }

        public virtual async Task StartAsync(CancellationToken cancellationToken)
        {
            await this.ScheduleJob(cancellationToken);
        }

        public virtual async Task DoWork(CancellationToken cancellationToken)
        {
            await Task.Delay(5000, cancellationToken);  // do the work
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            this.timer?.Stop();
            await Task.CompletedTask;
        }

        public virtual void Dispose()
        {
            this.timer?.Dispose();
        }

        protected virtual async Task ScheduleJob(CancellationToken cancellationToken)
        {
            var next = this.expression.GetNextOccurrence(DateTimeOffset.Now, this.timeZoneInfo);
            if (next.HasValue)
            {
                var delay = next.Value - DateTimeOffset.Now;
                if (delay.TotalDays <= 25)
                {
                    this.timer = new System.Timers.Timer(delay.TotalMilliseconds);
                    this.timer.Elapsed += async (sender, args) =>
                    {
                        this.timer.Stop();  // reset timer
                        await this.DoWork(cancellationToken);
                        await this.ScheduleJob(cancellationToken);    // reschedule next
                    };
                    this.timer.Start();
                }
                else
                {
                    this.timer = new System.Timers.Timer(86400000);
                    this.timer.Elapsed += async (sender, args) =>
                    {
                        this.timer.Stop();  // reset timer
                        await this.ScheduleJob(cancellationToken);    // reschedule next
                    };
                    this.timer.Start();
                }
            }

            await Task.CompletedTask;
        }
    }
}
