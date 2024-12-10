using KocCoAPI.Domain.Entities;
using KocCoAPI.Domain.Interfaces;

namespace KocCoAPI.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> AddUserAsync(User user)
        {
            return await _userRepository.AddAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }

        public async Task<bool> ExistsByUserMailAsync(string userMail)
        {
            return await _userRepository.ExistsByUserMailAsync(userMail);
        }

        public async Task<List<User>> GetAllUserAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetByUserIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
        }

        public async Task<User> GetByUserMailToUserAsync(string userMail)
        {
            return await _userRepository.GetByUserMailToUserAsync(userMail);
        }
        public async Task<List<Package>> GetUserPackagesByEmailAsync(string email)
        {
            return await _userRepository.GetUserPackagesByEmailAsync(email);
        }

        public async Task UpdatePackageAsync(Package package)
        {
            await _userRepository.UpdatePackageAsync(package);
        }

        public async Task<Package> GetPackageByIdAsync(int packageId)
        {
            return await _userRepository.GetPackageByIdAsync(packageId);
        }

        public async Task<decimal> GetCoachIncomeByEmailAsync(string email)
        {
            return await _userRepository.GetCoachIncomeByEmailAsync(email);
        }

        public async Task<List<User>> GetStudentsByCoachEmailAsync(string email)
        {
            return await _userRepository.GetStudentsByCoachEmailAsync(email);
        }

        public async Task<List<SharedResource>> GetSharedResourcesByCoachEmailAsync(string email)
        {
            return await _userRepository.GetSharedResourcesByCoachEmailAsync(email);
        }

        public async Task UploadSharedResourceAsync(string email, int packageId, string documentBase64, string documentName)
        {
            // Kullanıcının öğretmen olup olmadığını kontrol et
            //var isCoach = await _userRepository.IsCoachByEmailAsync(email);
            //if (!isCoach)
            //{
            //    throw new UnauthorizedAccessException("The user is not a coach.");
            //}

            // Yeni shared resource kaydet
            await _userRepository.AddSharedResourceAsync(packageId, documentBase64, documentName);
        }

        public async Task<List<SharedResource>> GetSharedResourcesForStudentAsync(string email, int packageId)
        {
            return await _userRepository.GetSharedResourcesForStudentAsync(email, packageId);
        }

        public async Task AddToCartAsync(string email, int packageId)
        {
            var user = await _userRepository.GetByUserMailToUserAsync(email);
            if (user == null) throw new InvalidOperationException("User not found.");

            await _userRepository.AddToCartAsync(user.UserId, packageId);
        }

        public async Task<List<CartPackage>> GetCartDetailsAsync(string email)
        {
            var user = await _userRepository.GetByUserMailToUserAsync(email);
            if (user == null) throw new InvalidOperationException("User not found.");

            return await _userRepository.GetCartDetailsAsync(user.CartId ?? 0);
        }

        public async Task<string> PurchaseCartAsync(string email, string cardDetails)
        {
            var user = await _userRepository.GetByUserMailToUserAsync(email);
            if (user == null) throw new InvalidOperationException("User not found.");

            return await _userRepository.PurchaseCartAsync(user.CartId ?? 0, user.UserId, cardDetails);
        }

        public async Task<List<Package>> GetAllPackagesAsync()
        {
            return await _userRepository.GetAllPackagesAsync();
        }

     

     

        public async Task<Test> GetTestByIdAsync(int testId)
        {
            return await _userRepository.GetTestByIdAsync(testId);
        }

       



        public async Task CreateWorkScheduleAsync(string email, string generalNotes)
        {
            // Fetch the user by email
            var user = await _userRepository.GetByUserMailToUserAsync(email);
            if (user == null)
            {
                throw new Exception("User not found for the provided email.");
            }

            // Create the WorkSchedule entity
            var workSchedule = new WorkSchedule
            {
                StudentId = user.UserId, // Fetch UserId from email
                GeneralNotes = generalNotes // Provided by frontend
            };

            // Save the WorkSchedule
            await _userRepository.CreateWorkScheduleAsync(workSchedule);
        }


        public async Task<List<WorkSchedule>> GetWorkSchedulesByEmailAsync(string email)
        {
            return await _userRepository.GetWorkSchedulesByEmailAsync(email);
        }
        
        public async Task<List<User>> GetAllCoachesAsync()
        {
            return await _userRepository.GetAllCoachesAsync();
        }

        public async Task AddTestResultAsync(string email, int testId, int grade)
        {
            // Email üzerinden UserId alınıyor
            var userId = await _userRepository.GetByUserMailToUserAsync(email);
            if (userId == null)
            {
                throw new Exception("User not found with the provided email.");
            }

            // TestResult nesnesi oluşturuluyor
            var testResult = new TestResult
            {
                TestId = testId,
                StudentId = userId.UserId,
                Grade = grade
            };

            // TestResult kaydı ekleniyor
            await _userRepository.AddTestResultAsync(testResult);
        }

        public async Task<List<TestResult>> GetTestResultsByStudentIdAsync(int studentId)
        {
            return await _userRepository.GetTestResultsByStudentIdAsync(studentId);
        }










    }
}
