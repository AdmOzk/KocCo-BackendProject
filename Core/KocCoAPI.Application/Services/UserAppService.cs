using AutoMapper;
using KocCoAPI.Application.DTOs;
using KocCoAPI.Application.Interfaces;
using KocCoAPI.Domain.Entities;
using KocCoAPI.Domain.Interfaces;

namespace KocCoAPI.Application.Services
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserAppService(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<UserDTO> AddUserAppAsync(UserDTO userDto)
        {
            var user = _mapper.Map<User>(userDto);
            var addedUser = await _userService.AddUserAsync(user);
            return _mapper.Map<UserDTO>(addedUser);
        }

        public async Task DeleteUserAppAsync(int id)
        {
            await _userService.DeleteUserAsync(id);
        }

        public async Task<bool> ExistsByUserMailAsync(string userMail)
        {
            return await _userService.ExistsByUserMailAsync(userMail);
        }

        public async Task<List<UserDTO>> GetAllUserAppAsync()
        {
            var users = await _userService.GetAllUserAsync();
            return _mapper.Map<List<UserDTO>>(users);
        }

        public async Task<UserDTO> GetByUserMailToUserAsync(string userMail)
        {
            var user = await _userService.GetByUserMailToUserAsync(userMail);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserInfoDTO> GetBasicInfoByUserMailAsync(string userMail)
        {
            var user = await _userService.GetByUserMailToUserAsync(userMail);
            return _mapper.Map<UserInfoDTO>(user);
        }

        public async Task<UserDTO> GetByUserAppIdAsync(int id)
        {
            var user = await _userService.GetByUserIdAsync(id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task UpdateUserAppAsync(UserDTO userDto)
        {
            var user = _mapper.Map<User>(userDto);
            await _userService.UpdateUserAsync(user);
        }

        public async Task<List<PackageDTO>> GetUserPackagesByEmailAsync(string userMail)
        {
            var userPackages = await _userService.GetUserPackagesByEmailAsync(userMail);
            return _mapper.Map<List<PackageDTO>>(userPackages);
        }

        public async Task<bool> UpdatePackageAsync(PackageDTO packageDto)
        {
            // DTO'dan Package nesnesine dönüşüm
            var package = _mapper.Map<Package>(packageDto);

            // Güncelleme işlemi
            await _userService.UpdatePackageAsync(package);
            return true;
        }

        public async Task<PackageDTO> GetPackageByIdAsync(int packageId)
        {
            var package = await _userService.GetPackageByIdAsync(packageId);

            if (package == null)
            {
                return null;
            }

            return _mapper.Map<PackageDTO>(package);
        }
    }
}
