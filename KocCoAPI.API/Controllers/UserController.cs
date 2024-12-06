using KocCoAPI.Application.DTOs;
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

        [HttpGet("get-package-by-id")]
        public async Task<IActionResult> GetPackageById([FromQuery] int packageId)
        {
            if (packageId <= 0)
            {
                return BadRequest(new { message = "Invalid package ID." });
            }

            try
            {
                var package = await _UserAppService.GetPackageByIdAsync(packageId);

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

        [HttpPut("update-package")]
        public async Task<IActionResult> UpdatePackage([FromBody] PackageDTO packageDto)
        {
            if (packageDto == null || packageDto.PackageID <= 0)
            {
                return BadRequest(new { message = "Invalid package details." });
            }

            try
            {
                await _UserAppService.UpdatePackageAsync(packageDto);

                return Ok(new { message = "Package updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("my-income")]
        public async Task<IActionResult> GetMyIncome([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { message = "Email is required." });
            }

            try
            {
                var income = await _UserAppService.GetCoachIncomeByEmailAsync(email);

                return Ok(new { email, totalIncome = income });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("get-students-by-coach-email")]
        public async Task<IActionResult> GetStudentsByCoachEmail([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { message = "Email is required." });
            }

            try
            {
                var students = await _UserAppService.GetStudentsByCoachEmailAsync(email);

                if (students == null || !students.Any())
                {
                    return NotFound(new { message = "No students found for this coach." });
                }

                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


    }
}
