// <copyright file="ILanguageService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAccess.Domains.Services
{
    public interface ILanguageService
    {
        string UserOrPasswordNotFound { get; }

        string UserNotExist { get; }

        string RecordNotCreated { get; }

        string RecordCreated { get; }

        string RecordNotUpdated { get; }

        string RecordUpdated { get; }

        string RecordNotDeleted { get; }

        string RecordDeleted { get; }
    }
}
