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











    }
}
