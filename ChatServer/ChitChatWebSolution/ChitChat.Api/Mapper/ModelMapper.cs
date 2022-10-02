using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Chat.Data.EfCore;
using ChitChat.Api.Dto;

namespace ChitChat.Api.Mapper
{
    public class ModelMapper: Profile
    {
        public ModelMapper()
        {
            CreateMap<UserDto, User>().ForPath(dest => dest.PasswordHash, 
                opt => opt.MapFrom(src => src.Password));

            CreateMap<User, UserDto>().ForPath(dest => dest.Username,
                opt => opt.MapFrom(src => src.Username));

            CreateMap<MessageDto, Message>().ReverseMap();
        }
       
    }
}
