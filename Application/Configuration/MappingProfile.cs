using AutoMapper;
using Entities.Owin;
using Services.DTO.Account;

namespace Application.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDTO, User>().ReverseMap();

        }
    }
}
