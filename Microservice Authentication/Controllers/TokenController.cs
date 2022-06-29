using AutoMapper;
using Microservice_Authentication.Data;
using Microservice_Authentication.Entities;
using Microservice_Authentication.Models;
using Microservice_Authentication.ServiceCalls;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice_Authentication.Controllers
{
    [ApiController]
    [Route("api/tokens")]
    [Produces("application/json", "application/xml")]
    public class TokenController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public TokenController(IUserService userService, ITokenService tokenService, IMapper mapper)
        {
            this._userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this._tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost, Authorize]
        [Route("revoke")]
        public IActionResult Revoke()
        {
            var username = User.Identity.Name;
            var userDTO = _userService.GetUserByUsername(username);

            if (userDTO == null)
            {
                return BadRequest();
            }

            UserEntity user = _mapper.Map<UserEntity>(userDTO);
            user.UserId = _userService.GetUserIdByUsername(username).Result;

            _userService.UpdateUser(user);
            if (user is not null)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
