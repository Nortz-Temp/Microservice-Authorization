using AutoMapper;
using Microservice_Authentication.Entities;
using Microservice_Authentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice_Authentication.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserEntity, UserFrontDTO>();
            CreateMap<UserFrontDTO, UserEntity>();
        }
    }
}
