// <copyright file="MyDBDao.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAccess.Providers
{
    using System;
    using System.Data.SqlClient;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using MyAccess.Domains.Providers;

    public class MyDBDao : IMyDBDao
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<MyDBDao> logger;
        private readonly SqlConnection sqlConnection;

        public MyDBDao(IConfiguration configuration, ILogger<MyDBDao> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.sqlConnection = new SqlConnection(this.configuration.GetConnectionString("DefaultConnection"));
        }

        public void Backup()
        {
            var backupFolder = this.configuration.GetSection("DB:BackupFolder")?.Value;
            var sqlConStrBuilder = new SqlConnectionStringBuilder(this.configuration.GetConnectionString("DefaultConnection"));

            var backupFileName = $"{backupFolder}{sqlConStrBuilder.InitialCatalog}-{DateTime.Now:yyyy-MM-dd}.bak";

            var query = $"BACKUP DATABASE {sqlConStrBuilder.InitialCatalog} TO  DISK = N'{backupFileName}' WITH NOFORMAT, NOINIT,  NAME = N'{sqlConStrBuilder.InitialCatalog}-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";

            using var command = new SqlCommand(query, this.sqlConnection);
            this.sqlConnection.Open();
            command.ExecuteNonQuery();
            this.sqlConnection.Close();
        }
    }
}
