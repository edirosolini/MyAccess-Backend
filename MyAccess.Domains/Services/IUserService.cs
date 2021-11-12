// <copyright file="IUserService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAccess.Domains.Services
{
    using System;
    using System.Collections.Generic;
    using MyAccess.Domains.Requests;
    using MyAccess.Domains.Responses;

    public interface IUserService : IDisposable
    {
        AuthenticateResponse Authenticate(AuthenticateRequest request);

        void ChangePassword(ChangePasswordRequest request);

        void Creaate(UserRequest request);

        GenericResponse<IEnumerable<UserRequest>> Get(int since, int limit);

        void Update(UserRequest request);

        void Delete(UserRequest request);
    }
}
