using KocCoAPI.Application.DTOs;
using KocCoAPI.Application.Interfaces;
using KocCoAPI.Application.Services;
using KocCoAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KocCoAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
   
    public class PackageController : ControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly IPackageAppService _packageAppService; 

        public PackageController(IUserAppService userAppService, IPackageAppService packageAppService)
        {
            _userAppService = userAppService;
            _packageAppService = packageAppService;
        }

        [Authorize(Roles = "User,Coach")]
        [HttpGet("get-all-packages")]
        public async Task<IActionResult> GetAllPackages()
        {
            try
            {
                var packages = await _packageAppService.GetAllPackagesAsync();

                if (packages == null || !packages.Any())
                {
                    return NotFound(new { message = "No packages found." });
                }

                return Ok(packages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize(Roles = "User")]
        [HttpGet("get-my-packages")]
        public async Task<IActionResult> GetMyPackages()
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }

            try
            {
                var packages = await _packageAppService.GetUserPackagesByEmailAsync(email);
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

        [Authorize(Roles = "Coach")]
        [HttpGet("get-coach-packages")]
        public async Task<IActionResult> GetCoachPackages()
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }

            try
            {
                // Kullanıcının rolünü kontrol etmek için UserService'i kullanıyoruz
                var user = await _userAppService.GetByUserMailToUserAsync(email);
                if (user == null || user.Roles != "Coach")
                {
                    return NotFound(new { message = "Coach not found or invalid role." });
                }

                // Koçun paketlerini almak
                var packages = await _packageAppService.GetCoachPackagesAsync(email);
                if (packages == null || !packages.Any())
                {
                    return NotFound(new { message = "No packages found for this coach." });
                }

                return Ok(packages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize(Roles = "User,Coach")]
        [HttpGet("get-package-by-id")]
        public async Task<IActionResult> GetPackageById([FromQuery] int packageId)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }

            if (packageId <= 0)
            {
                return BadRequest(new { message = "Invalid package ID." });
            }

            try
            {
                var package = await _packageAppService.GetPackageByIdAsync(packageId);

                if (package == null)
                {
                    return NotFound(new { message = "Package not found." });
                }

                return Ok(package);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Coach")]
        [HttpPut("update-package")]
        public async Task<IActionResult> UpdatePackage([FromBody] PackageDTO packageDto)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }

            if (packageDto == null || packageDto.PackageID <= 0)
            {
                return BadRequest(new { message = "Invalid package details." });
            }

            try
            {
                await _packageAppService.UpdatePackageAsync(packageDto);

                return Ok(new { message = "Package updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Coach")]
        [HttpGet("get-specific-user-packages")]
        public async Task<IActionResult> GetUserPackagesByEmail([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { message = "Email is required." });
            }

            try
            {
                var packages = await _packageAppService.GetUserPackagesByEmailRequestAsync(email);

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

        [Authorize(Roles = "Coach")]
        [HttpPost("add-package-by-coach")]
        public async Task<IActionResult> AddPackageByCoach([FromBody] PackageDTO packageDto)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }

            if (packageDto == null || string.IsNullOrEmpty(packageDto.PackageName) || packageDto.Price <= 0 || packageDto.DurationInDays <= 0)
            {
                return BadRequest(new { message = "Invalid package details." });
            }

            try
            {
                await _packageAppService.AddPackageByCoachAsync(email, packageDto);
                return Ok(new { message = "Package added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Coach")]
        [HttpDelete("remove-package")]
        public async Task<IActionResult> RemovePackage([FromQuery] int packageId)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized(new { message = "Unauthorized request." });
            }

            if (packageId <= 0)
            {
                return BadRequest(new { message = "Invalid package ID." });
            }

            try
            {
                await _packageAppService.RemovePackageAsync(email, packageId);
                return Ok(new { message = "Package removed successfully." });
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