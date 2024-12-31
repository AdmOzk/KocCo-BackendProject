using KocCoAPI.Application.DTOs;
using KocCoAPI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KocCoAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var result = await _authService.LoginAsync(loginRequestDTO);

            if (!result.Success)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }
    }
}
