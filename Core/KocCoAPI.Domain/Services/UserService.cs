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






    }
}
