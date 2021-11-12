// <copyright file="UserService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAccess.Services
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using AutoMapper;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;
    using MyAccess.Commons;
    using MyAccess.Domains.Entities;
    using MyAccess.Domains.Providers;
    using MyAccess.Domains.Requests;
    using MyAccess.Domains.Responses;
    using MyAccess.Domains.Services;

    public class UserService : IUserService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<UserService> logger;
        private readonly IMapper mapper;
        private readonly IUserDao userDao;

        private readonly ILanguageService languageService;

        public UserService(IConfiguration configuration, ILogger<UserService> logger, IMapper mapper, IUserDao userDao, ILanguageService languageService)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.mapper = mapper;
            this.userDao = userDao;
            this.languageService = languageService;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest request)
        {
            var user = this.userDao.Get().SingleOrDefault(x => x.EmailAddress == request.EmailAddress && PasswordHash.ValidatePassword(request.Password, x.Password));

            // return null if user not found
            if (user == null)
            {
                var e = new Exception(this.languageService.UserOrPasswordNotFound);
                this.logger.LogError(e, e.Message);
                throw e;
            }

            var response = this.mapper.Map<AuthenticateResponse>(user);
            this.GenerateJwtToken(response);

            return response;
        }

        public void ChangePassword(ChangePasswordRequest request)
        {
            var user = this.userDao.Get().SingleOrDefault(x => x.Id == request.UserId && PasswordHash.ValidatePassword(request.CurrentPassword, x.Password));

            // return null if user not found
            if (user != null)
            {
                user.Password = PasswordHash.CreateHash(request.NewPassword);

                try
                {
                    this.userDao.Update(user);
                    this.logger.LogInformation(this.languageService.RecordUpdated);
                }
                catch (Exception e)
                {
                    this.logger.LogError(e, this.languageService.RecordNotUpdated);
                    throw;
                }
            }
            else
            {
                var e = new Exception(this.languageService.UserNotExist);
                this.logger.LogError(e, e.Message);
                throw e;
            }
        }

        public void Creaate(UserRequest request)
        {
            try
            {
                var userEntity = this.mapper.Map<UserEntity>(request);
                this.userDao.Insert(userEntity);

                this.logger.LogInformation(this.languageService.RecordCreated);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, this.languageService.RecordNotCreated);
                throw;
            }
        }

        public void Delete(UserRequest request)
        {
            try
            {
                this.userDao.Delete(this.userDao.Get(request.Id));
                this.logger.LogInformation(this.languageService.RecordDeleted);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, this.languageService.RecordNotDeleted);
                throw;
            }
        }

        public void Dispose()
        {
            this.userDao.Dispose();
        }

        public GenericResponse<IEnumerable<UserRequest>> Get(int since, int limit)
        {
            var users = this.userDao.Get();

            var data = from u in users
                       orderby u.EmailAddress
                       select new UserRequest
                       {
                           Id = u.Id,
                           LastName = u.LastName,
                           FirstName = u.FirstName,
                           EmailAddress = u.EmailAddress,
                       };

            if (limit == 0)
            {
                return new GenericResponse<IEnumerable<UserRequest>>
                {
                    Count = data.Count(),
                    Data = data
                        .Skip(since),
                };
            }
            else
            {
                return new GenericResponse<IEnumerable<UserRequest>>
                {
                    Count = data.Count(),
                    Data = data
                        .Skip(since)
                        .Take(limit),
                };
            }
        }

        public void Update(UserRequest request)
        {
            try
            {
                var userEntity = this.mapper.Map<UserEntity>(request);
                userEntity.Password = this.userDao.Get(userEntity.Id)?.Password;
                this.userDao.Update(userEntity);

                this.logger.LogInformation(this.languageService.RecordUpdated);
            }
            catch (Exception e)
            {
                this.logger.LogError(e, this.languageService.RecordNotUpdated);
                throw;
            }
        }

        private void GenerateJwtToken(AuthenticateResponse response)
        {
            var signingKey = this.configuration.GetSection("SecurityToken:IssuerSigningKey");
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey.Value));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var header = new JwtHeader(signingCredentials);

            var claims = new[]
            {
                new Claim("Id", response.Id.ToString()),
                new Claim("BusinessName", $"{response.BusinessName}"),
                new Claim(ClaimTypes.Email, $"{response.EmailAddress}"),
                new Claim(ClaimTypes.Role, $"{response.Rol}"),
            };

            var payload = new JwtPayload(
                issuer: this.configuration.GetSection("SecurityToken:Issuer")?.Value,
                audience: this.configuration.GetSection("SecurityToken:Audience")?.Value,
                claims: claims,
                notBefore: null,
                expires: DateTime.UtcNow.AddDays(1));

            var token = new JwtSecurityToken(
                    header,
                    payload);

            response.SetTokenAccess(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
