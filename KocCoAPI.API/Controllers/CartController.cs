using KocCoAPI.Application.DTOs;
using KocCoAPI.Application.Interfaces;
using KocCoAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KocCoAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "User")]
    public class CartController : ControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly ICartAppService _cartAppService;

        public CartController(IUserAppService userAppService, ICartAppService cartAppService)
        {
            _userAppService = userAppService;
            _cartAppService = cartAppService;
        }

        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddToCart([FromQuery] int packageId)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }

            if (packageId <= 0)
            {
                return BadRequest(new { message = "Cart id can't find." });
            }

            try
            {
                await _cartAppService.AddToCartAsync(email, packageId);
                return Ok(new { message = "Package added to cart successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("view-cart")]
        public async Task<IActionResult> ViewCart()
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }

            try
            {
                var cart = await _cartAppService.GetCartDetailsAsync(email);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("purchase-cart")]
        public async Task<IActionResult> PurchaseCart([FromBody] string cardDetails)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }

            if (string.IsNullOrEmpty(cardDetails))
            {
                return BadRequest(new { message = "Email and CardDetails are required." });
            }

            try
            {
                var result = await _cartAppService.PurchaseCartAsync(email, cardDetails);
                return Ok(new { message = result });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("remove-from-cart")]
        public async Task<IActionResult> RemoveFromCart([FromQuery] int packageId)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }

            if (packageId <= 0)
            {
                return BadRequest(new { message = "Valid package ID is required." });
            }

            try
            {
                await _cartAppService.RemoveFromCartAsync(email, packageId);
                return Ok(new { message = "Package removed from cart successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }
}
