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

        Task<List<Package>> GetCoachPackagesAsync(string email);

        Task UpdatePackageAsync(Package package);

        Task<Package> GetPackageByIdAsync(int packageId);
        Task<decimal> GetCoachIncomeByEmailAsync(string email);
        Task<List<User>> GetStudentsByCoachEmailAsync(string email);

        Task<List<SharedResource>> GetSharedResourcesByCoachEmailAsync(string email);

        Task AddSharedResourceAsync(int packageId, string documentBase64, string documentName);

        Task<List<SharedResource>> GetSharedResourcesForStudentAsync(string email, int packageId);

        Task AddToCartAsync(int userId, int packageId);
        Task<List<CartPackage>> GetCartDetailsAsync(int cartId);
        Task<string> PurchaseCartAsync(int cartId, int userId, string cardDetails);

        Task<List<Package>> GetAllPackagesAsync();



        Task<Test> GetTestByIdAsync(int testId);
        Task UpdateTestAsync(Test test);

        Task CreateWorkScheduleAsync(WorkSchedule workSchedule);

        Task<List<WorkSchedule>> GetWorkSchedulesByEmailAsync(string email);

        Task<List<User>> GetAllCoachesAsync();

        Task AddTestResultAsync(TestResult testResult);
        Task<List<TestResult>> GetTestResultsByStudentIdAsync(int studentId);

    }
}
