using KocCoAPI.Domain.Entities;

namespace KocCoAPI.Domain.Interfaces
{
    public interface ICartRepository
    {
        Task AddToCartAsync(int userId, int packageId);
        Task<List<CartPackage>> GetCartDetailsAsync(int cartId);
        Task<string> PurchaseCartAsync(int cartId, int userId, string cardDetails);
        Task RemoveFromCartAsync(int cartId, int packageId);

    }
}
