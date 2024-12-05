using KocCoAPI.Domain.Entities;
using KocCoAPI.Domain.Interfaces;
using KocCoAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KocCoAPI.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlDbContext _dbContext;
        public UserRepository(SqlDbContext context)
        {
            _dbContext = context;
        }

        public async Task<User> AddAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task DeleteAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task UpdateAsync(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<bool> ExistsByUserMailAsync(string userMail)
        {
            return await _dbContext.Users.AnyAsync(u => u.EmailAddress == userMail);
        }

        

        public async Task<User> GetByUserMailToUserAsync(string userMail)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.EmailAddress == userMail);
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

    }
}
