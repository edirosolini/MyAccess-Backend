// <copyright file="LanguageService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAccess.Services
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using MyAccess.Domains.Services;

    public class LanguageService : ILanguageService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<LanguageService> logger;

        public string UserOrPasswordNotFound { get; }

        public string UserNotExist { get; }

        public string RecordNotUpdated { get; }

        public string RecordUpdated { get; }

        public string RecordNotCreated { get; }

        public string RecordCreated { get; }

        public string RecordNotDeleted { get; }

        public string RecordDeleted { get; }

        public LanguageService(IConfiguration configuration, ILogger<LanguageService> logger)
        {
            this.configuration = configuration;
            this.logger = logger;

            switch (this.configuration.GetSection("Language")?.Value)
            {
                case "ES":
                    this.UserOrPasswordNotFound = "No se encontró el usuario y/o la contraseña.";
                    this.UserNotExist = "El usuario no existe.";
                    this.RecordNotUpdated = "Registro no actualizado.";
                    this.RecordUpdated = "Registro actualizado.";
                    this.RecordNotCreated = "Registro no creado.";
                    this.RecordCreated = "Registro creado.";
                    this.RecordNotDeleted = "Registro eliminado.";
                    this.RecordDeleted = "Registro no eliminado.";
                    break;
                default:
                    this.UserOrPasswordNotFound = "User and/or password not found.";
                    this.UserNotExist = "Username does not exist.";
                    this.RecordNotUpdated = "Record not updated.";
                    this.RecordUpdated = "Record updated.";
                    this.RecordNotCreated = "Record created.";
                    this.RecordCreated = "Record not created.";
                    this.RecordNotDeleted = "Record deleted.";
                    this.RecordDeleted = "Record not deleted.";
                    break;
            }
        }
    }
}
