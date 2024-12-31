using KocCoAPI.Domain.Entities;

namespace KocCoAPI.Domain.Interfaces
{
    public interface ICartService
    {
        Task AddToCartAsync(string email, int packageId);
        Task<List<CartPackage>> GetCartDetailsAsync(string email);
        Task<string> PurchaseCartAsync(string email, string cardDetails);
        Task RemoveFromCartAsync(string email, int packageId);
    }
}
