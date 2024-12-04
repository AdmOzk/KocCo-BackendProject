using KocCoAPI.Application.Interfaces;
using KocCoAPI.Application.Services;
using KocCoAPI.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace KocCoAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserAppService _UserAppService;
        

        public UserController(IUserAppService UserAppService)
        {
            _UserAppService = UserAppService;
         
        }

        [HttpGet("get-by-email")]
        public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { message = "Email is required." });
            }

            try
            {
                var user = await _UserAppService.GetByUserMailToUserAsync(email);
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("get-userInfo")]
        public async Task<IActionResult> GetBasicInfoByEmail([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { message = "Email is required." });
            }

            try
            {
                var userBasicInfo = await _UserAppService.GetBasicInfoByUserMailAsync(email);
                if (userBasicInfo == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                return Ok(userBasicInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("get-my-packages")]
        public async Task<IActionResult> GetMyPackages([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { message = "Email is required." });
            }

            try
            {
                var packages = await _UserAppService.GetUserPackagesByEmailAsync(email);
                if (packages == null || !packages.Any())
                {
                    return NotFound(new { message = "No packages found for this user." });
                }

                return Ok(packages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


    }
}
