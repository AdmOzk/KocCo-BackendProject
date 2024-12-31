using KocCoAPI.Application.DTOs;

namespace KocCoAPI.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegisterDTO registerDto);
        Task<AuthResult> LoginAsync(LoginRequestDTO loginRequestDTO);
    }
}
