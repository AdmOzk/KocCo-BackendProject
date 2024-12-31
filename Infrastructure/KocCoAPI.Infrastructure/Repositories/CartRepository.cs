using KocCoAPI.Domain.Entities;
using KocCoAPI.Domain.Interfaces;
using KocCoAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KocCoAPI.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly SqlDbContext _dbContext;
        public CartRepository(SqlDbContext context)
        {
            _dbContext = context;
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
            return $"Payment successful. You paid {totalPrice} TL.";
        }

        public async Task RemoveFromCartAsync(int cartId, int packageId)
        {
            var cartPackage = await _dbContext.CartPackages
                .FirstOrDefaultAsync(cp => cp.CartId == cartId && cp.PackageId == packageId);

            if (cartPackage == null)
            {
                throw new InvalidOperationException("Package not found in the cart.");
            }

            // Update the total price in the cart
            var cart = await _dbContext.Carts.FirstOrDefaultAsync(c => c.CartId == cartId);
            if (cart != null)
            {
                var package = await _dbContext.Packages.FirstOrDefaultAsync(p => p.PackageID == packageId);
                if (package != null)
                {
                    cart.TotalPrice -= package.Price;
                    _dbContext.Carts.Update(cart);
                }
            }

            // Remove the package from the cart
            _dbContext.CartPackages.Remove(cartPackage);

            await _dbContext.SaveChangesAsync();
        }

    }
}
