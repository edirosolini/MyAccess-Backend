// <copyright file="MyDBService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAccess.Services
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using MyAccess.Domains.Providers;
    using MyAccess.Domains.Services;

    public class MyDBService : IMyDBService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<MyDBService> logger;

        private readonly IMyDBDao dao;

        public MyDBService(IConfiguration configuration, ILogger<MyDBService> logger, IMyDBDao dao)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.dao = dao;
        }

        public void Backup() => this.dao.Backup();
    }
}
