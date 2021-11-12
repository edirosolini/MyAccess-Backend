// <copyright file="IRepository.cs.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAccess.Domains.Providers
{
    using System;
    using System.Collections.Generic;
    using MyAccess.Domains.Entities;

    public interface IRepository<T> : IDisposable
        where T : BaseEntity
    {
        long Insert(T entity);

        long Insert(IEnumerable<T> entities);

        T Get(Guid id);

        IEnumerable<T> Get();

        bool Update(T entity);

        bool Update(IEnumerable<T> entities);

        bool Delete(T entity);

        bool Delete(IEnumerable<T> entities);
    }
}
