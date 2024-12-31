using AutoMapper;
using KocCoAPI.Application.DTOs;
using KocCoAPI.Application.Interfaces;
using KocCoAPI.Domain.Entities;
using KocCoAPI.Domain.Interfaces;
using KocCoAPI.Domain.Services;

namespace KocCoAPI.Application.Services
{
    public class CartAppService : ICartAppService
    {
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;

        public CartAppService(ICartService cartService, IMapper mapper)
        {
            _cartService = cartService;
            _mapper = mapper;
        }


        public async Task AddToCartAsync(string email, int packageId)
        {
            await _cartService.AddToCartAsync(email, packageId);
        }

        public async Task<List<CartDTO>> GetCartDetailsAsync(string email)
        {
            var cartPackages = await _cartService.GetCartDetailsAsync(email);
            return _mapper.Map<List<CartDTO>>(cartPackages);
        }

        public async Task<string> PurchaseCartAsync(string email, string cardDetails)
        {
            return await _cartService.PurchaseCartAsync(email, cardDetails);
        }

        public async Task RemoveFromCartAsync(string email, int packageId)
        {
            await _cartService.RemoveFromCartAsync(email, packageId);
        }



    }
}
