using Microservice_Authentication.Entities;
using Microservice_Authentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice_Authentication.ServiceCalls
{
    public interface IUserService
    {
        public Task<UserFrontDTO> GetUserByUsername(AuthEntity principal);

        public Task<Guid> GetUserIdByUsername(string username);

        public Task<PasswordDTO> GetPasswordSalt(Guid passwordId);

        public Task<UserFrontDTO> UpdateUser(UserEntity user);
    }
}
