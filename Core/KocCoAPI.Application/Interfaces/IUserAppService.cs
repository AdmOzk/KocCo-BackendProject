using KocCoAPI.Application.DTOs;

namespace KocCoAPI.Application.Interfaces
{
    public interface IUserAppService
    {
        Task<UserDTO> AddUserAppAsync(UserDTO userDto);
        Task<List<UserDTO>> GetAllUserAppAsync();
        Task<UserDTO> GetByUserAppIdAsync(int id);
        Task UpdateUserAppAsync(UserDTO userDto);
        Task DeleteUserAppAsync(int id);
        Task<bool> ExistsByUserMailAsync(string userMail);
        Task<UserDTO> GetByUserMailToUserAsync(string userMail);
        Task<UserInfoDTO> GetBasicInfoByUserMailAsync(string userMail);
        Task<List<PackageDTO>> GetUserPackagesByEmailAsync(string userMail);
        Task<PackageDTO> GetPackageByIdAsync(int packageId);
        Task<bool> UpdatePackageAsync(PackageDTO packageDto);
        Task<decimal> GetCoachIncomeByEmailAsync(string email);

        Task<List<UserSimpleInfoDTO>> GetStudentsByCoachEmailAsync(string email);

    }
}
