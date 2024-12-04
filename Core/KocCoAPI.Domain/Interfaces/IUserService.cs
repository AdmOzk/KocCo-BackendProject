using KocCoAPI.Domain.Entities;

namespace KocCoAPI.Domain.Interfaces
{
    public interface IUserService
    {
        Task<User> AddUserAsync(User user);
        Task<List<User>> GetAllUserAsync();
        Task<User> GetByUserIdAsync(int id);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<bool> ExistsByUserMailAsync(string userMail);
        Task<User> GetByUserMailToUserAsync(string userMail);
        Task<List<Package>> GetUserPackagesByEmailAsync(string email);
    }
}
