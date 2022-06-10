using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice_Authentication.Models
{
    public class UserDTO
    {
        /// <summary>
        /// User first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// User last name
        /// </summary>

        public string LastName { get; set; }

        /// <summary>
        /// User username
        /// </summary>

        public string Username { get; set; }

        /// <summary>
        /// User email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User password id
        /// </summary>
        public Guid PasswordId { get; set; }

        /// <summary>
        /// User password id
        /// </summary>
        public string HashedPassword { get; set; }

        /// <summary>
        /// User usertype id
        /// </summary>
        public Guid UserTypeId { get; set; }

        /*/// <summary>
        /// User usertype dto
        /// </summary>
        public UserTypeDTO UserType { get; set; }*/

        /// <summary>
        /// User usertype name
        /// </summary>
        public string UserTypeName { get; set; }

        /// <summary>
        /// Refresh token
        /// </summary>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// Refresh token expiry time
        /// </summary>
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
