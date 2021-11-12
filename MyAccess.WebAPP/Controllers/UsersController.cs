// <copyright file="UsersController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAccess.WebAPP.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using MyAccess.Domains.Requests;
    using MyAccess.Domains.Responses;
    using MyAccess.Domains.Services;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<UsersController> logger;
        private readonly IUserService service;

        public UsersController(IConfiguration configuration, ILogger<UsersController> logger, IUserService service)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.service = service;
        }

        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        public ActionResult<AuthenticateResponse> Authenticate([FromForm] AuthenticateRequest request)
        {
            var authenticate = this.service.Authenticate(request);

            if (authenticate == null)
            {
                return this.Unauthorized();
            }

            return this.Ok(authenticate);
        }

        [HttpPut]
        [Route("[action]")]
        public void ChangePassword([FromBody] ChangePasswordRequest request) => this.service.ChangePassword(request);

        [HttpPost]
        public void Insert([FromBody] UserRequest request) => this.service.Creaate(request);

        [HttpGet]
        public GenericResponse<IEnumerable<UserRequest>> Get([FromQuery] int since = 0, [FromQuery] int limit = 25) => this.service.Get(since, limit);

        [HttpPut]
        public void Update([FromBody] UserRequest request) => this.service.Update(request);

        [HttpDelete]
        public void Delete([FromBody] UserRequest request) => this.service.Delete(request);
    }
}
