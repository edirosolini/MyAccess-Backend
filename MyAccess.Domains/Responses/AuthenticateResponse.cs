// <copyright file="AuthenticateResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAccess.Domains.Responses
{
    using System;

    public class AuthenticateResponse : BaseResponse
    {
        private string tokenAccess;

        public Guid Id { get; set; }

        public string BusinessName { get; set; }

        public string EmailAddress { get; set; }

        public string Rol { get; set; }

        public string TokenType { get; } = "Bearer";

        public string TokenAccess => this.tokenAccess;

        public void SetTokenAccess(string tokenAccess)
        {
            this.tokenAccess = tokenAccess;
        }
    }
}
