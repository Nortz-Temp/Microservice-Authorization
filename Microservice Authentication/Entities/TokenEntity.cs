using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice_Authentication.Entities
{
    public class TokenEntity
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
