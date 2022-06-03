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

        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh(TokenEntity tokenApiModel)
        {
            if (tokenApiModel is null)
                return BadRequest("Invalid client request");

            string accessToken = tokenApiModel.AccessToken;
            string refreshToken = tokenApiModel.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity.Name;
            var userDTO = _userService.GetUserByUsername(username);

            if (userDTO is null || userDTO.Result.RefreshToken != refreshToken || userDTO.Result.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid client request");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            userDTO.Result.RefreshToken = newRefreshToken;
            UserEntity user = _mapper.Map<UserEntity>(userDTO.Result);

            user.UserId = _userService.GetUserIdByUsername(username).Result;

            UserDTO editedUser = _userService.UpdateUser(user).Result;

            if (editedUser is null)
            {
                return BadRequest("Invalid client request");
            }

            return Ok(new AuthenticatedResponseDTO()
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            });
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

            user.RefreshToken = null;
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
