using KocCoAPI.Application.DTOs;


namespace KocCoAPI.Application.Interfaces
{
    public interface ICartAppService
    {

        Task AddToCartAsync(string email, int packageId);
        Task<List<CartDTO>> GetCartDetailsAsync(string email);
        Task<string> PurchaseCartAsync(string email, string cardDetails);
        Task RemoveFromCartAsync(string email, int packageId);

    }
}
