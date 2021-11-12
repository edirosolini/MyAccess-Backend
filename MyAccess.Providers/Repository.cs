// <copyright file="Repository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAccess.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using Dapper.Contrib.Extensions;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using MyAccess.Domains.Entities;
    using MyAccess.Domains.Providers;

    public class Repository<T> : IRepository<T>
        where T : BaseEntity
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<IRepository<T>> logger;
        private readonly SqlConnection sqlConnection;

        public Repository(IConfiguration configuration, ILogger<IRepository<T>> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.sqlConnection = new SqlConnection(this.configuration.GetConnectionString("DefaultConnection"));
        }

        public bool Delete(T entity) => this.sqlConnection.Delete(entity);

        public bool Delete(IEnumerable<T> entities) => this.sqlConnection.Delete(entities);

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(true);
        }

        public T Get(Guid id) => this.sqlConnection.Get<T>(id);

        public IEnumerable<T> Get() => this.sqlConnection.GetAll<T>();

        public long Insert(T entity) => this.sqlConnection.Insert(entity);

        public long Insert(IEnumerable<T> entities) => this.sqlConnection.Insert(entities);

        public bool Update(T entity)
        {
            entity.ModificationDate = DateTime.UtcNow;
            return this.sqlConnection.Update(entity);
        }

        public bool Update(IEnumerable<T> entities)
        {
            entities.ToList().ForEach(e => e.ModificationDate = DateTime.UtcNow);
            return this.sqlConnection.Update(entities);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.sqlConnection != null)
                {
                    this.sqlConnection.Dispose();
                }
            }
        }
    }
}
