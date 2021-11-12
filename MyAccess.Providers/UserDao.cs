// <copyright file="UserDao.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAccess.Providers
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using MyAccess.Domains.Entities;
    using MyAccess.Domains.Providers;

    public class UserDao : Repository<UserEntity>, IUserDao
    {
        public UserDao(IConfiguration configuration, ILogger<UserDao> logger)
                  : base(configuration, logger)
        {
        }
    }
}
