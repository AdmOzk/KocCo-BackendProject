using KocCoAPI.Application.DTOs;
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

        public async Task<decimal> GetCoachIncomeByEmailAsync(string email)
        {
            // User tablosunu kullanarak CoachPackage ve UserPurchases tablolarını ilişkilendirin
            var totalIncome = await _dbContext.CoachPackages
                .Join(
                    _dbContext.Users, // Users tablosu ile ilişkilendir
                    cp => cp.CoachId, // CoachPackages'deki CoachId
                    u => u.UserId,    // Users'daki UserId
                    (cp, u) => new { CoachPackage = cp, CoachEmail = u.EmailAddress } // Koçun email bilgisi ile eşleştir
                )
                .Where(x => x.CoachEmail == email) // Koçun emailine göre filtrele
                .Join(
                    _dbContext.UserPurchases, // UserPurchases tablosu ile ilişkilendir
                    x => x.CoachPackage.PackageId, // CoachPackages'teki PackageId
                    up => up.PackageID,            // UserPurchases'teki PackageID
                    (x, up) => up.PackageID        // Paket ID'sini eşleştir
                )
                .Join(
                    _dbContext.Packages, // Paket detaylarını al
                    packageId => packageId,
                    p => p.PackageID,
                    (packageId, p) => p.Price // Paketin fiyatını al
                )
                .SumAsync(price => price); // Fiyatları topla

            return totalIncome;
        }

        public async Task<List<User>> GetStudentsByCoachEmailAsync(string email)
        {
            return await _dbContext.CoachPackages
                .Join(
                    _dbContext.Users, // Users tablosuyla CoachPackages'ı ilişkilendiriyoruz
                    cp => cp.CoachId,
                    u => u.UserId,
                    (cp, u) => new { CoachPackage = cp, CoachEmail = u.EmailAddress }
                )
                .Where(x => x.CoachEmail == email) // Koçun email adresine göre filtreleme
                .Join(
                    _dbContext.UserPurchases, // CoachPackage ile UserPurchases'ı ilişkilendiriyoruz
                    x => x.CoachPackage.PackageId,
                    up => up.PackageID,
                    (x, up) => up.StudentID // Satın alan öğrencilerin ID'sini alıyoruz
                )
                .Join(
                    _dbContext.Users, // Öğrencilerin detay bilgilerini almak için Users tablosunu birleştiriyoruz
                    studentId => studentId,
                    u => u.UserId,
                    (studentId, u) => u // Tüm öğrenci detaylarını seçiyoruz
                )
                .ToListAsync();
        }

        public async Task<List<SharedResource>> GetSharedResourcesByCoachEmailAsync(string email)
        {
            return await _dbContext.CoachPackages
                .Join(
                    _dbContext.Users, // CoachPackages ile Users tablosunu ilişkilendir
                    cp => cp.CoachId,
                    u => u.UserId,
                    (cp, u) => new { CoachPackage = cp, CoachEmail = u.EmailAddress }
                )
                .Where(x => x.CoachEmail == email) // Koçun emailine göre filtreleme
                .Join(
                    _dbContext.SharedResources, // CoachPackages ile SharedResources'ı ilişkilendir
                    x => x.CoachPackage.PackageId,
                    sr => sr.PackageId,
                    (x, sr) => sr // SharedResource nesnesini al
                )
                .ToListAsync();
        }

        public async Task AddSharedResourceAsync(int packageId, string documentBase64, string documentName)
        {
            // Check if the document name already exists for the given package
            bool exists = await _dbContext.SharedResources
                .AnyAsync(sr => sr.PackageId == packageId && sr.DocumentName == documentName);

            if (exists)
            {
                throw new InvalidOperationException("This document name is already used. Please choose a different name.");
            }

            // Create a new shared resource record
            var sharedResource = new SharedResource
            {
                PackageId = packageId,
                Document = documentBase64,
                DocumentName = documentName // User-provided document name
            };

            _dbContext.SharedResources.Add(sharedResource);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<SharedResource>> GetSharedResourcesForStudentAsync(string email, int packageId)
        {
            return await _dbContext.UserPurchases
                .Where(up => up.User.EmailAddress == email && up.PackageID == packageId) // Filter by email and package
                .Join(
                    _dbContext.SharedResources, // Join with SharedResources
                    up => up.PackageID,
                    sr => sr.PackageId,
                    (up, sr) => sr // Select the shared resources
                )
                .ToListAsync();
        }

        public async Task<List<CartPackage>> GetCartDetailsAsync(int cartId)
        {
            return await _dbContext.CartPackages
                .Include(cp => cp.Package) // Fetch package details
                .Include(cp => cp.Cart)    // Fetch cart details
                .Where(cp => cp.CartId == cartId)
                .ToListAsync();
        }

        public async Task AddToCartAsync(int userId, int packageId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null) throw new InvalidOperationException("User not found.");

            var cart = await _dbContext.Carts.FirstOrDefaultAsync(c => c.CartId == user.CartId);
            if (cart == null)
            {
                cart = new Cart { TotalPrice = 0 };
                _dbContext.Carts.Add(cart);
                await _dbContext.SaveChangesAsync();
                user.CartId = cart.CartId;
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();
            }

            var package = await _dbContext.Packages.FirstOrDefaultAsync(p => p.PackageID == packageId);
            if (package == null) throw new InvalidOperationException("Package not found.");

            _dbContext.CartPackages.Add(new CartPackage { CartId = cart.CartId, PackageId = packageId });
            cart.TotalPrice += package.Price;
            _dbContext.Carts.Update(cart);

            await _dbContext.SaveChangesAsync();
        }

        private bool ProcessPayment(string cardDetails, decimal amount)
        {
            // Simulate payment processing
            return true; // Always return success for now
        }


        public async Task<string> PurchaseCartAsync(int cartId, int userId, string cardDetails)
        {
            // Sepetteki paketleri alıyoruz.
            var cartPackages = await _dbContext.CartPackages
                .Where(cp => cp.CartId == cartId)
                .Include(cp => cp.Package)
                .ToListAsync();

            if (!cartPackages.Any())
            {
                throw new InvalidOperationException("Cart is empty.");
            }

            // Toplam fiyatı hesapla.
            decimal totalPrice = cartPackages.Sum(cp => cp.Package.Price);

            // Ödeme işlemini simüle et.
            if (!ProcessPayment(cardDetails, totalPrice))
            {
                throw new InvalidOperationException("Payment failed.");
            }

            // Kullanıcının satın alımlarına ekle.
            foreach (var cartPackage in cartPackages)
            {
                _dbContext.UserPurchases.Add(new UserPurchased
                {
                    StudentID = userId,
                    PackageID = cartPackage.PackageId
                });
            }

            // Sepeti temizle.
            _dbContext.CartPackages.RemoveRange(cartPackages);
            var cart = await _dbContext.Carts.FirstOrDefaultAsync(c => c.CartId == cartId);
            if (cart != null)
            {
                cart.TotalPrice = 0; // Toplam fiyatı sıfırla.
                _dbContext.Carts.Update(cart);
            }

            await _dbContext.SaveChangesAsync();
            return $"Payment successful. You paid {totalPrice} USD.";
        }

        public async Task<List<Package>> GetAllPackagesAsync()
        {
            return await _dbContext.Packages.ToListAsync();
        }



    }
}
