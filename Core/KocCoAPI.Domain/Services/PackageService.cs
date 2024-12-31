using KocCoAPI.Domain.Entities;
using KocCoAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KocCoAPI.Domain.Services
{
    public class PackageService : IPackageService
    {
        private readonly IPackageRepository _packageRepository;

        public PackageService(IPackageRepository packageRepository)
        {
            _packageRepository = packageRepository;
        }

        public async Task<List<Package>> GetUserPackagesByEmailAsync(string email)
        {
            return await _packageRepository.GetUserPackagesByEmailAsync(email);
        }

        public async Task<List<Package>> GetCoachPackagesAsync(string email)
        {
            return await _packageRepository.GetCoachPackagesAsync(email);
        }

        public async Task UpdatePackageAsync(Package package)
        {
            await _packageRepository.UpdatePackageAsync(package);
        }

        public async Task<Package> GetPackageByIdAsync(int packageId)
        {
            return await _packageRepository.GetPackageByIdAsync(packageId);
        }

        public async Task<List<Package>> GetAllPackagesAsync()
        {
            return await _packageRepository.GetAllPackagesAsync();
        }

        public async Task<List<Package>> GetUserPackagesByEmailRequestAsync(string email)
        {
            return await _packageRepository.GetUserPackagesByEmailRequestAsync(email);
        }

        public async Task AddPackageByCoachAsync(string email, Package package)
        {
            var coach = await _packageRepository.GetCoachByEmailAsync(email);
            if (coach == null || coach.Roles != "Coach")
                throw new InvalidOperationException("Coach not found or invalid role.");

            package.CreatedAt = DateTime.UtcNow;
            package.UpdatedAt = DateTime.UtcNow;

            await _packageRepository.AddPackageByCoachAsync(coach.UserId, package);
        }

        public async Task RemovePackageAsync(string email, int packageId)
        {
            // Ensure the coach owns the package
            var coachPackages = await _packageRepository.GetCoachPackagesAsync(email);
            var package = coachPackages.FirstOrDefault(p => p.PackageID == packageId);

            if (package == null)
            {
                throw new InvalidOperationException("You are not authorized to remove this package, or the package does not exist.");
            }

            // Delete the package
            await _packageRepository.RemovePackageAsync(packageId);
        }




    }
}
