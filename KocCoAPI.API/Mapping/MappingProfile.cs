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

            CreateMap<User, CoachInfoDTO>().ReverseMap();

            CreateMap<User, UserInfoDTO>().ReverseMap();

            CreateMap<Package, PackageDTO>().ReverseMap();

            CreateMap<User, UserSimpleInfoDTO>().ReverseMap();

            CreateMap<SharedResource, SharedResourceDTO>().ReverseMap();

            CreateMap<Cart, CartDTO>().ReverseMap();

            CreateMap<Test, TestDTO>().ReverseMap();


            CreateMap<WorkScheduleDTO, WorkSchedule>().ReverseMap();

            CreateMap<WorkScheduleDTO, WorkSchedule>()
           .ForMember(dest => dest.WorkScheduleId, opt => opt.Ignore()) // Auto-incremented by DB
           .ForMember(dest => dest.StudentId, opt => opt.Ignore()); // Will be set manually
        


            CreateMap<CartPackage, CartDTO>()
    .ForMember(dest => dest.PackageId, opt => opt.MapFrom(src => src.PackageId))
    .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.Package.PackageName))
    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Package.Price))
    .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Cart.TotalPrice));
        }



    }
}
