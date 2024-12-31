using KocCoAPI.Domain.Entities;

namespace KocCoAPI.Domain.Interfaces
{
    public interface IPackageService
    {
        Task<List<Package>> GetUserPackagesByEmailAsync(string email);
        Task<List<Package>> GetCoachPackagesAsync(string email);
        Task UpdatePackageAsync(Package package);
        Task<Package> GetPackageByIdAsync(int packageId);

        Task<List<Package>> GetAllPackagesAsync();

        Task<List<Package>> GetUserPackagesByEmailRequestAsync(string email);

        Task AddPackageByCoachAsync(string email, Package package);

        Task RemovePackageAsync(string email, int packageId);
    }
}
