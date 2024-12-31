using KocCoAPI.Domain.Entities;
using KocCoAPI.Domain.Interfaces;

namespace KocCoAPI.Domain.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUserRepository _userRepository;
        public CartService(ICartRepository cartRepository, IUserRepository userRepository)
        {
            _cartRepository = cartRepository;
            _userRepository = userRepository;
        }

        public async Task AddToCartAsync(string email, int packageId)
        {
            var user = await _userRepository.GetByUserMailToUserAsync(email);
            if (user == null) throw new InvalidOperationException("User not found.");

            await _cartRepository.AddToCartAsync(user.UserId, packageId);
        }

        public async Task<List<CartPackage>> GetCartDetailsAsync(string email)
        {
            var user = await _userRepository.GetByUserMailToUserAsync(email);
            if (user == null) throw new InvalidOperationException("User not found.");

            return await _cartRepository.GetCartDetailsAsync(user.CartId ?? 0);
        }

        public async Task<string> PurchaseCartAsync(string email, string cardDetails)
        {
            var user = await _userRepository.GetByUserMailToUserAsync(email);
            if (user == null) throw new InvalidOperationException("User not found.");

            return await _cartRepository.PurchaseCartAsync(user.CartId ?? 0, user.UserId, cardDetails);
        }

        public async Task RemoveFromCartAsync(string email, int packageId)
        {
            var user = await _userRepository.GetByUserMailToUserAsync(email);
            if (user == null) throw new InvalidOperationException("User not found.");

            if (user.CartId == null) throw new InvalidOperationException("User does not have an active cart.");

            await _cartRepository.RemoveFromCartAsync(user.CartId.Value, packageId);
        }


    }
}
