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

        public async Task<List<PackageDTO>> GetCoachPackagesAsync(string email)
        {
            // Kullanıcının paketlerini repository üzerinden al
            var coachPackages = await _userService.GetCoachPackagesAsync(email);

            // Listeyi DTO'ya dönüştür
            return _mapper.Map<List<PackageDTO>>(coachPackages);
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

        public async Task<decimal> GetCoachIncomeByEmailAsync(string email)
        {
            return await _userService.GetCoachIncomeByEmailAsync(email);
        }

        public async Task<List<UserSimpleInfoDTO>> GetStudentsByCoachEmailAsync(string email)
        {
            var students = await _userService.GetStudentsByCoachEmailAsync(email);

            return _mapper.Map<List<UserSimpleInfoDTO>>(students); // User nesnelerini DTO'ya dönüştürüyoruz
        }

        public async Task<List<SharedResourceDTO>> GetSharedResourcesByCoachEmailAsync(string email)
        {
            // SharedResource listesini al
            var sharedResources = await _userService.GetSharedResourcesByCoachEmailAsync(email);

            // SharedResource listesini DTO'ya dönüştür
            return sharedResources.Select(sr => new SharedResourceDTO
            {
                DocumentName = sr.DocumentName,
                Document = sr.Document // Doküman içeriği
            }).ToList();
        }

        public async Task UploadSharedResourceAsync(string email, int packageId, string documentBase64, string documentName)
        {
            // Servis katmanına yönlendir
            await _userService.UploadSharedResourceAsync(email, packageId, documentBase64, documentName);
        }

        public async Task<List<SharedResourceDTO>> GetSharedResourcesForStudentAsync(string email, int packageId)
        {
            // Call the service layer to retrieve shared resources
            var sharedResources = await _userService.GetSharedResourcesForStudentAsync(email, packageId);

            // Map the data to DTO
            return _mapper.Map<List<SharedResourceDTO>>(sharedResources);
        }

        public async Task AddToCartAsync(string email, int packageId)
        {
            await _userService.AddToCartAsync(email, packageId);
        }

        public async Task<List<CartDTO>> GetCartDetailsAsync(string email)
        {
            var cartPackages = await _userService.GetCartDetailsAsync(email);
            return _mapper.Map<List<CartDTO>>(cartPackages);
        }

        public async Task<string> PurchaseCartAsync(string email, string cardDetails)
        {
            return await _userService.PurchaseCartAsync(email, cardDetails);
        }

        public async Task<List<PackageDTO>> GetAllPackagesAsync()
        {
            var packages = await _userService.GetAllPackagesAsync();
            return _mapper.Map<List<PackageDTO>>(packages);
        }

        public async Task<TestDTO> GetTestByIdAsync(int testId)
        {
            var test = await _userService.GetTestByIdAsync(testId);
            return _mapper.Map<TestDTO>(test);
        }

        public async Task<WorkScheduleDTO> CreateWorkScheduleAsync(WorkScheduleDTO workScheduleDTO)
        {
            if (string.IsNullOrEmpty(workScheduleDTO.Email))
            {
                throw new ArgumentException("Email is required.");
            }

            // Delegate to the service
            await _userService.CreateWorkScheduleAsync(workScheduleDTO.Email, workScheduleDTO.GeneralNotes);
            return _mapper.Map<WorkScheduleDTO>(workScheduleDTO);
        }

        public async Task<List<WorkScheduleDTO>> GetWorkSchedulesByEmailAsync(string email)
        {
            // Domain'den WorkSchedule entity'sini al
            var workSchedules = await _userService.GetWorkSchedulesByEmailAsync(email);

            // WorkSchedule entity'lerini DTO'ya dönüştür
            return workSchedules.Select(ws => new WorkScheduleDTO
            {
                Email = email, // Direkt olarak kullanıcının email adresi
                GeneralNotes = ws.GeneralNotes
            }).ToList();
        }


        public async Task<List<CoachInfoDTO>> GetAllCoachesAsync()
        {
            var Coaches = await _userService.GetAllCoachesAsync();
            return _mapper.Map<List<CoachInfoDTO>>(Coaches);
        }

        public async Task AddTestResultAsync(string email, int testId, int grade)
        {
            await _userService.AddTestResultAsync(email, testId, grade);
        }

        public async Task<List<TestResultDTO>> GetTestResultsByEmailAsync(string email)
        {
            // Kullanıcının StudentId'sini al
            var user = await _userService.GetByUserMailToUserAsync(email);
            if (user == null)
                throw new Exception("User not found");

            // Test sonuçlarını al
            var testResults = await _userService.GetTestResultsByStudentIdAsync(user.UserId);

            // TestName ve StudentName bilgilerini doldur
            var testResultsWithDetails = new List<TestResultDTO>();
            foreach (var result in testResults)
            {
                // Test bilgisini al
                var test = await _userService.GetTestByIdAsync(result.TestId);

                // Öğrenci adını al
                var student = await _userService.GetByUserIdAsync(result.StudentId);

                testResultsWithDetails.Add(new TestResultDTO
                {
                    TestResultId = result.TestResultId,
                    TestId = result.TestId,
                    StudentId = result.StudentId,
                    Grade = result.Grade,
                    TestName = test?.TestName, // TestName olabilir
                    StudentName = $"{student?.FirstName} {student?.LastName}" // Full Name
                });
            }

            return testResultsWithDetails;
        }



    }
}
