using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice_Authentication.Models
{
    public class PasswordDTO
    {
        /// <summary>
        /// Password salt
        /// </summary>
        public string Salt { get; set; }
    }
}
