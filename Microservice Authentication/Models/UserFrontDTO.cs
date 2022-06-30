namespace Microservice_Authentication.Models
{
    public class UserFrontDTO
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
        /// User usertype id
        /// </summary>
        public string UserType { get; set; }
    }
}
