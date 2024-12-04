using AutoMapper;
using KocCoAPI.Application.DTOs;
using KocCoAPI.Application.Interfaces;
using KocCoAPI.Domain.Entities;
using KocCoAPI.Domain.Interfaces;

namespace KocCoAPI.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserAppService _userAppService;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        public AuthService(IUserAppService userAppService, ITokenService tokenService, IPasswordHasher passwordHasher, IMapper mapper)
        {
            _userAppService = userAppService;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<AuthResult> LoginAsync(LoginDTO loginDto)
        {
            var userExist = await _userAppService.ExistsByUserMailAsync(loginDto.EmailAdress);

            if (userExist)
            {
                var user = await _userAppService.GetByUserMailToUserAsync(loginDto.EmailAdress);

                if (user == null || !_passwordHasher.VerifyPassword(user.PasswordHash, loginDto.Password))
                {
                    return new AuthResult { Success = false, Errors = new[] { "Invalid email or password." } };
                }

                var userDto = _mapper.Map<UserDTO>(user);

                var token = _tokenService.GenerateToken(userDto);
                var refreshToken = _tokenService.GenerateRefreshToken();

                return new AuthResult
                {
                    Success = true,
                    Token = token,
                    RefreshToken = refreshToken.Token
                };
            }
            else
            {
                return new AuthResult
                {
                    Success = false,
                    Token = "Error",
                    RefreshToken = "Error"
                };
            }
        }

        public async Task<AuthResult> RegisterAsync(RegisterDTO registerDto)
        {
            if (await _userAppService.ExistsByUserMailAsync(registerDto.EmailAddress))
            {
                return new AuthResult { Success = false, Errors = new[] { "User already in use." } };
            }

            var hashedPassword = _passwordHasher.HashPassword(registerDto.PasswordHash);

            var userDto = new UserDTO
            {
                Gender = registerDto.Gender,
                FirstName = registerDto.FirstName,
                EmailAddress = registerDto.EmailAddress,
                LastName = registerDto.LastName,
                DateOfBirth = registerDto.DateOfBirth,
                PhoneNumber = registerDto.PhoneNumber,
                Skills = registerDto.Skills,
                Roles = "User",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                PasswordHash = hashedPassword,
            };

            await _userAppService.AddUserAppAsync(userDto);

            var token = _tokenService.GenerateToken(userDto);
            var refreshToken = _tokenService.GenerateRefreshToken();

            return new AuthResult
            {
                Success = true,
                Token = token,
                RefreshToken = refreshToken.Token
            };
        }
    }
}
