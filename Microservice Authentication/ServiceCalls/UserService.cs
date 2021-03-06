using Microservice_Authentication.Entities;
using Microservice_Authentication.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microservice_Authentication.ServiceCalls
{
    public class UserService : IUserService
    {
        private readonly IConfiguration configuration;

        public UserService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<UserFrontDTO> GetUserByUsername(AuthEntity principal)
        {
            using (HttpClient client = new HttpClient())
            {
                Uri url = new Uri($"{ configuration["Services:UserService"] }api/users/auth/username");
                HttpContent content = new StringContent(JsonConvert.SerializeObject(principal));
                content.Headers.ContentType.MediaType = "application/json";

                HttpResponseMessage response = client.PostAsync(url, content).Result;

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new UserFrontDTO();
                }
                var responseContent = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserFrontDTO>(responseContent);

                return user;
            }
        }

        public async Task<Guid> GetUserIdByUsername(string username)
        {
            using (HttpClient client = new HttpClient())
            {
                var x = configuration["Services:UserService"];
                Uri url = new Uri($"{ configuration["Services:UserService"] }api/users/GetIdByUsername/{username}");

                HttpResponseMessage response = client.GetAsync(url).Result;
                var responseContent = await response.Content.ReadAsStringAsync();
                var userId = JsonConvert.DeserializeObject<Guid>(responseContent);

                return userId;
            }
        }

        public async Task<UserFrontDTO> UpdateUser(UserEntity user)
        {
            using (HttpClient client = new HttpClient())
            {
                var x = configuration["Services:UserService"];
                Uri url = new Uri($"{ configuration["Services:UserService"] }api/users");

                HttpContent content = new StringContent(JsonConvert.SerializeObject(user));
                content.Headers.ContentType.MediaType = "application/json";

                HttpResponseMessage response = client.PutAsync(url.ToString(), content).Result;
                var responseContent = await response.Content.ReadAsStringAsync();
                var userDTO = JsonConvert.DeserializeObject<UserFrontDTO>(responseContent);

                return userDTO;
            }
        }

        public async Task<PasswordDTO> GetPasswordSalt(Guid passwordId)
        {
            using (HttpClient client = new HttpClient())
            {
                var x = configuration["Services:UserService"];
                Uri url = new Uri($"{ configuration["Services:UserService"] }api/passwords/{passwordId}");

                HttpResponseMessage response = client.GetAsync(url).Result;
                var responseContent = await response.Content.ReadAsStringAsync();
                var passwordSalt = JsonConvert.DeserializeObject<PasswordDTO>(responseContent);

                return passwordSalt;
            }
        }
    }
}
