using KocCoAPI.Application.DTOs;
using KocCoAPI.Domain.Entities;
using KocCoAPI.Domain.Interfaces;
using KocCoAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KocCoAPI.Infrastructure.Repositories
{
    public class PackageRepository : IPackageRepository
    {
        private readonly SqlDbContext _dbContext;
        public PackageRepository(SqlDbContext context)
        {
            _dbContext = context;
        }

        public async Task<List<Package>> GetUserPackagesByEmailAsync(string email)
        {
            return await _dbContext.UserPurchases
                .Include(up => up.Package) // Package ile ilişkiyi dahil ediyoruz
                .Include(up => up.User) // User ile ilişkiyi dahil ediyoruz
                .Where(up => up.User.EmailAddress == email) // Kullanıcı emailine göre filtreleme
                .Select(up => up.Package) // Sadece Package nesnesini seçiyoruz
                .ToListAsync();
        }

        public async Task<List<Package>> GetCoachPackagesAsync(string email)
        {
            return await (from cp in _dbContext.CoachPackages
                          join u in _dbContext.Users on cp.CoachId equals u.UserId
                          join p in _dbContext.Packages on cp.PackageId equals p.PackageID
                          where u.EmailAddress == email && u.Roles == "Coach"
                          select p).ToListAsync();
        }

        public async Task UpdatePackageAsync(Package package)
        {
            var existingPackage = await _dbContext.Packages
                .FirstOrDefaultAsync(p => p.PackageID == package.PackageID);

            if (existingPackage != null)
            {
                existingPackage.PackageName = package.PackageName;
                existingPackage.Description = package.Description;
                existingPackage.Price = package.Price;
                existingPackage.DurationInDays = package.DurationInDays;
                existingPackage.Rating = package.Rating;

                _dbContext.Packages.Update(existingPackage);
                await _dbContext.SaveChangesAsync();
            }
        }


        public async Task<Package> GetPackageByIdAsync(int packageId)
        {
            return await _dbContext.Packages
                .FirstOrDefaultAsync(p => p.PackageID == packageId);
        }

        public async Task<List<Package>> GetAllPackagesAsync()
        {
            return await _dbContext.Packages.ToListAsync();
        }

        public async Task<List<Package>> GetUserPackagesByEmailRequestAsync(string email)
        {
            return await (from u in _dbContext.Users
                          join up in _dbContext.UserPurchases on u.UserId equals up.StudentID
                          join p in _dbContext.Packages on up.PackageID equals p.PackageID
                          where u.EmailAddress == email
                          select p).ToListAsync();
        }

        public async Task<User> GetCoachByEmailAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.EmailAddress == email && u.Roles == "Coach");
        }

        public async Task AddPackageByCoachAsync(int coachId, Package package)
        {
            // Add the package to the Packages table
            await _dbContext.Packages.AddAsync(package);
            await _dbContext.SaveChangesAsync();

            // Add the Coach-Package relationship
            var coachPackage = new CoachPackage
            {
                CoachId = coachId,
                PackageId = package.PackageID // PackageID is auto-generated
            };

            await _dbContext.CoachPackages.AddAsync(coachPackage);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemovePackageAsync(int packageId)
        {
            // Remove the package from the Packages table
            var package = await _dbContext.Packages.FirstOrDefaultAsync(p => p.PackageID == packageId);
            if (package != null)
            {
                _dbContext.Packages.Remove(package);
            }

            // Remove the corresponding record from the CoachPackages table
            var coachPackage = await _dbContext.CoachPackages.FirstOrDefaultAsync(cp => cp.PackageId == packageId);
            if (coachPackage != null)
            {
                _dbContext.CoachPackages.Remove(coachPackage);
            }

            // Save the changes
            await _dbContext.SaveChangesAsync();
        }



    }
}
