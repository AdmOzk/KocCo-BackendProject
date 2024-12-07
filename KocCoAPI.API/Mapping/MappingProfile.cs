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

            CreateMap<SharedResource, SharedResourceDTO>().ReverseMap();

            CreateMap<Cart, CartDTO>().ReverseMap();

            CreateMap<CartPackage, CartDTO>()
    .ForMember(dest => dest.PackageId, opt => opt.MapFrom(src => src.PackageId))
    .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.Package.PackageName))
    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Package.Price))
    .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Cart.TotalPrice));
        }


    }
}
