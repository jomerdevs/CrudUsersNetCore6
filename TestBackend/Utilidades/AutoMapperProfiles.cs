using AutoMapper;
using TestBackend.DTOs;
using TestBackend.Models;

namespace TestBackend.Utilidades
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CreateUserDTO, User>();
            CreateMap<PatchUserDTO, User>().ReverseMap();
            CreateMap<RegisterDTO, User>();
            CreateMap<LoginDTO, User>();
        }
    }
}
