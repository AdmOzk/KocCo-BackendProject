using KocCoAPI.Application.DTOs;
using KocCoAPI.Domain.Interfaces;

namespace KocCoAPI.Application.Interfaces
{
    public interface IPackageAppService
    {
        Task<List<PackageDTO>> GetUserPackagesByEmailAsync(string userMail);
        Task<List<PackageDTO>> GetCoachPackagesAsync(string email);
        Task<PackageDTO> GetPackageByIdAsync(int packageId);
        Task<bool> UpdatePackageAsync(PackageDTO packageDto);

        Task<List<PackageDTO>> GetAllPackagesAsync();

        Task<List<UserPackageDTO>> GetUserPackagesByEmailRequestAsync(string email);

        Task AddPackageByCoachAsync(string email, PackageDTO packageDto);

        Task RemovePackageAsync(string email, int packageId);


    }
}
