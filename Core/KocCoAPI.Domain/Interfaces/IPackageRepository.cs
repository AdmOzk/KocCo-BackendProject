using KocCoAPI.Domain.Entities;

namespace KocCoAPI.Domain.Interfaces
{
    public interface IPackageRepository
    {
        Task<List<Package>> GetUserPackagesByEmailAsync(string email);

        Task<List<Package>> GetCoachPackagesAsync(string email);

        Task UpdatePackageAsync(Package package);

        Task<Package> GetPackageByIdAsync(int packageId);

        Task<List<Package>> GetAllPackagesAsync();

        Task<List<Package>> GetUserPackagesByEmailRequestAsync(string email);

        Task<User> GetCoachByEmailAsync(string email);
        Task AddPackageByCoachAsync(int coachId, Package package);

        Task RemovePackageAsync(int packageId);

    }
}
