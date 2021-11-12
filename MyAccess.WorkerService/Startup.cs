// <copyright file="Startup.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAccess.WorkerService
{
    using Microsoft.AspNetCore.Builder;
    using MyAccess.DependencyInjection;

    internal class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UpdateDatabase();
        }
    }
}