using AutoMapper;
using KocCoAPI.Application.DTOs;
using KocCoAPI.Application.Interfaces;
using KocCoAPI.Domain.Entities;
using KocCoAPI.Domain.Interfaces;
using KocCoAPI.Domain.Services;

namespace KocCoAPI.Application.Services
{
    public class PackageAppService : IPackageAppService
    {
        private readonly IPackageService _packageService;
        private readonly IMapper _mapper;

        public PackageAppService(IPackageService packageService, IMapper mapper)
        {
            _packageService = packageService;
            _mapper = mapper;
        }


        public async Task<List<PackageDTO>> GetUserPackagesByEmailAsync(string userMail)
        {
            var userPackages = await _packageService.GetUserPackagesByEmailAsync(userMail);
            return _mapper.Map<List<PackageDTO>>(userPackages);
        }

        public async Task<List<PackageDTO>> GetCoachPackagesAsync(string email)
        {
            // Kullanıcının paketlerini repository üzerinden al
            var coachPackages = await _packageService.GetCoachPackagesAsync(email);

            // Listeyi DTO'ya dönüştür
            return _mapper.Map<List<PackageDTO>>(coachPackages);
        }

        public async Task<bool> UpdatePackageAsync(PackageDTO packageDto)
        {
            // DTO'dan Package nesnesine dönüşüm
            var package = _mapper.Map<Package>(packageDto);

            // Güncelleme işlemi
            await _packageService.UpdatePackageAsync(package);
            return true;
        }

        public async Task<PackageDTO> GetPackageByIdAsync(int packageId)
        {
            var package = await _packageService.GetPackageByIdAsync(packageId);

            if (package == null)
            {
                return null;
            }

            return _mapper.Map<PackageDTO>(package);
        }

        public async Task<List<PackageDTO>> GetAllPackagesAsync()
        {
            var packages = await _packageService.GetAllPackagesAsync();
            return _mapper.Map<List<PackageDTO>>(packages);
        }

        public async Task<List<UserPackageDTO>> GetUserPackagesByEmailRequestAsync(string email)
        {
            // Service katmanından Entity listesini al
            var packages = await _packageService.GetUserPackagesByEmailRequestAsync(email);

            // Fazlalık veriyi temizlemek için DTO'ya map et
            return packages
                .Select(p => new UserPackageDTO
                {
                    PackageID = p.PackageID,
                    PackageName = p.PackageName
                })
                .Distinct() // Benzersiz hale getir
                .ToList();
        }

        public async Task AddPackageByCoachAsync(string email, PackageDTO packageDto)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email is required.");

            if (string.IsNullOrEmpty(packageDto.PackageName) || packageDto.Price <= 0 || packageDto.DurationInDays <= 0)
                throw new ArgumentException("Invalid package details.");

            await _packageService.AddPackageByCoachAsync(email, _mapper.Map<Package>(packageDto));
        }

        public async Task RemovePackageAsync(string email, int packageId)
        {
            // Delegate to the service layer
            await _packageService.RemovePackageAsync(email, packageId);
        }



    }
}
