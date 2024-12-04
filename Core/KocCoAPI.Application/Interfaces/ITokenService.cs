using KocCoAPI.Application.DTOs;
using KocCoAPI.Domain.Entities;

namespace KocCoAPI.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(UserDTO userDto);
        RefreshToken GenerateRefreshToken();
    }
}
