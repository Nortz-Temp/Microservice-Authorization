using AutoMapper;
using Microservice_Authentication.Data;
using Microservice_Authentication.Entities;
using Microservice_Authentication.Models;
using Microservice_Authentication.ServiceCalls;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Microservice_Authentication.Controllers
{
    [ApiController]
    [Route("api/auths")]
    [Produces("application/json", "application/xml")]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly static int iterations = 1000;
        public AuthController(IUserService userContext, ITokenService tokenService, IMapper mapper)
        {
            _userService = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost, Route("login")]
        public IActionResult Login([FromBody] AuthEntity loginModel)
        {
            try
            {
                if (loginModel is null)
                {
                    return BadRequest("Invalid client request");
                }
                var userDTO = _userService.GetUserByUsername(loginModel.UserName);

                var response = userDTO.Result;

                var passwordSalt = _userService.GetPasswordSalt(response.PasswordId).Result;

                if (userDTO.Status == TaskStatus.Faulted || userDTO is null)
                    return Unauthorized();

                if (!VerifyPassword(loginModel.Password, response.HashedPassword, passwordSalt.Salt))
                    return Unauthorized();

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginModel.UserName),
                new Claim(ClaimTypes.Role, response.UserTypeName)
            };

                var accessToken = _tokenService.GenerateAccessToken(claims);
                var refreshToken = _tokenService.GenerateRefreshToken();
                userDTO.Result.RefreshToken = refreshToken;
                userDTO.Result.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);

                UserEntity user = _mapper.Map<UserEntity>(userDTO.Result);

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);
                user.UserId = _userService.GetUserIdByUsername(loginModel.UserName).Result;

                UserDTO editedUser = _userService.UpdateUser(user).Result;

                if (editedUser is null)
                {
                    return BadRequest("Invalid client request");
                }

                return Ok(new AuthenticatedResponseDTO
                {
                    Token = accessToken,
                    RefreshToken = refreshToken
                });
            }catch(Exception e)
            {
                return StatusCode(500, e.StackTrace);
            }
           
        }

        private bool VerifyPassword(string password, string savedHash, string savedSalt)
        {
            var saltBytes = Convert.FromBase64String(savedSalt);
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, iterations);
            if (Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256)) == savedHash)
            {
                return true;
            }
            return false;
        }
    }
}
