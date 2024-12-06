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
        Task UpdatePackageAsync(Package package);
        Task<Package> GetPackageByIdAsync(int packageId);
        Task<decimal> GetCoachIncomeByEmailAsync(string email);

        Task<List<User>> GetStudentsByCoachEmailAsync(string email);

        Task<List<SharedResource>> GetSharedResourcesByCoachEmailAsync(string email);

        Task UploadSharedResourceAsync(string email, int packageId, string documentBase64, string documentName);

        Task<List<SharedResource>> GetSharedResourcesForStudentAsync(string email, int packageId);

    }
}
