using KocCoAPI.Domain.Entities;

namespace KocCoAPI.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user);
        Task<List<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
        Task<bool> ExistsByUserMailAsync(string userMail);
        Task<User> GetByUserMailToUserAsync(string userMail);

        Task<List<Package>> GetUserPackagesByEmailAsync(string email);
    }
}
