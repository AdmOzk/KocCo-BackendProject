using AutoMapper;
using KocCoAPI.Application.DTOs;
using KocCoAPI.Domain.Entities;

namespace KocCoAPI.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();

            CreateMap<User, UserInfoDTO>().ReverseMap();

            CreateMap<Package, PackageDTO>().ReverseMap();

            CreateMap<User, UserSimpleInfoDTO>().ReverseMap();
        }


    }
}
