// <copyright file="MyDbContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAccess.Providers
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using MyAccess.Domains.Entities;
    using MyAccess.Providers.Mappers;

    public class MyDbContext : DbContext
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<MyDbContext> logger;

        public DbSet<UserEntity> Users { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> option, IConfiguration configuration, ILogger<MyDbContext> logger)
            : base(option)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            this.logger.LogInformation("Applying entity settings");

            modelBuilder.ApplyConfiguration(new UserMapper());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(x => x.MigrationsHistoryTable("__MigrationsHistory"));
        }
    }
}
