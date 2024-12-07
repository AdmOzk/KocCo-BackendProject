using KocCoAPI.Application.DTOs;
using KocCoAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KocCoAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly IUserAppService _userAppService;

        public CartController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddToCart([FromQuery] string email, [FromQuery] int packageId)
        {
            if (string.IsNullOrEmpty(email) || packageId <= 0)
            {
                return BadRequest(new { message = "Email and PackageId are required." });
            }

            try
            {
                await _userAppService.AddToCartAsync(email, packageId);
                return Ok(new { message = "Package added to cart successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("view-cart")]
        public async Task<IActionResult> ViewCart([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { message = "Email is required." });
            }

            try
            {
                var cart = await _userAppService.GetCartDetailsAsync(email);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("purchase-cart")]
        public async Task<IActionResult> PurchaseCart([FromQuery] string email, [FromBody] string cardDetails)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(cardDetails))
            {
                return BadRequest(new { message = "Email and CardDetails are required." });
            }

            try
            {
                var result = await _userAppService.PurchaseCartAsync(email, cardDetails);
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
    }
}
