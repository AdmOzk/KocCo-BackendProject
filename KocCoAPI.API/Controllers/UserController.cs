using KocCoAPI.Application.DTOs;
using KocCoAPI.Application.Interfaces;
using KocCoAPI.Application.Services;
using KocCoAPI.Domain.Interfaces;
using KocCoAPI.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace KocCoAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserAppService _UserAppService;
        

        public UserController(IUserAppService UserAppService)
        {
            _UserAppService = UserAppService;
         
        }

        [HttpGet("get-by-email")]
        public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { message = "Email is required." });
            }

            try
            {
                var user = await _UserAppService.GetByUserMailToUserAsync(email);
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("get-userInfo")]
        public async Task<IActionResult> GetBasicInfoByEmail([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { message = "Email is required." });
            }

            try
            {
                var userBasicInfo = await _UserAppService.GetBasicInfoByUserMailAsync(email);
                if (userBasicInfo == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                return Ok(userBasicInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }



        [HttpGet("get-my-packages")]
        public async Task<IActionResult> GetMyPackages([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { message = "Email is required." });
            }

            try
            {
                var packages = await _UserAppService.GetUserPackagesByEmailAsync(email);
                if (packages == null || !packages.Any())
                {
                    return NotFound(new { message = "No packages found for this user." });
                }

                return Ok(packages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("get-package-by-id")]
        public async Task<IActionResult> GetPackageById([FromQuery] int packageId)
        {
            if (packageId <= 0)
            {
                return BadRequest(new { message = "Invalid package ID." });
            }

            try
            {
                var package = await _UserAppService.GetPackageByIdAsync(packageId);

                if (package == null)
                {
                    return NotFound(new { message = "Package not found." });
                }

                return Ok(package);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("update-package")]
        public async Task<IActionResult> UpdatePackage([FromBody] PackageDTO packageDto)
        {
            if (packageDto == null || packageDto.PackageID <= 0)
            {
                return BadRequest(new { message = "Invalid package details." });
            }

            try
            {
                await _UserAppService.UpdatePackageAsync(packageDto);

                return Ok(new { message = "Package updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("my-income")]
        public async Task<IActionResult> GetMyIncome([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { message = "Email is required." });
            }

            try
            {
                var income = await _UserAppService.GetCoachIncomeByEmailAsync(email);

                return Ok(new { email, totalIncome = income });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("get-students-by-coach-email")]
        public async Task<IActionResult> GetStudentsByCoachEmail([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { message = "Email is required." });
            }

            try
            {
                var students = await _UserAppService.GetStudentsByCoachEmailAsync(email);

                if (students == null || !students.Any())
                {
                    return NotFound(new { message = "No students found for this coach." });
                }

                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("get-shared-resources")]
        public async Task<IActionResult> GetSharedResources([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { message = "Email is required." });
            }

            try
            {
                // SharedResourceDTO listesini al
                var resources = await _UserAppService.GetSharedResourcesByCoachEmailAsync(email);

                if (resources == null || !resources.Any())
                {
                    return NotFound(new { message = "No shared resources found for this coach." });
                }

                return Ok(resources); // DTO'ları döndür
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("download-document")]
        public async Task<IActionResult> DownloadDocument([FromQuery] string email, [FromQuery] string documentName)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(documentName))
            {
                return BadRequest(new { message = "Email and document name are required." });
            }

            try
            {
                // SharedResourceDTO listesini alın
                var resources = await _UserAppService.GetSharedResourcesByCoachEmailAsync(email);

                // Belirtilen dokümanı bul
                var resource = resources.FirstOrDefault(r => r.DocumentName == documentName);

                if (resource == null)
                {
                    return NotFound(new { message = "Document not found." });
                }

                // Base64'ü byte array'e dönüştür
                var documentBytes = Convert.FromBase64String(resource.Document);

                // Tarayıcıya dosyayı indirme talimatı ver
                return File(documentBytes, "application/pdf", $"{documentName}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("upload-file")]
        public async Task<IActionResult> UploadFile(IFormFile file, [FromQuery] string documentName, [FromQuery] int packageId, [FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email) || packageId <= 0 || file == null || string.IsNullOrEmpty(documentName))
            {
                return BadRequest(new { message = "Fill all data." });
            }

           

            // Dosya boyutu kontrolü
            long size = file.Length;
            if (size > (20 * 1024 * 1024)) // 20 MB
            {
                return BadRequest(new { message = "Maximum size can be 20MB." });
            }


            var memoryStream = new MemoryStream();
            var documentBase64 = "";
            try
            {
                // Convert file to Base64 string
                await file.CopyToAsync(memoryStream);
                documentBase64 = Convert.ToBase64String(memoryStream.ToArray());

                // Upload the shared resource
                await _UserAppService.UploadSharedResourceAsync(email, packageId, documentBase64, documentName);

                return Ok(new { message = "Document uploaded successfully." });
            }
            catch (InvalidOperationException ex)
            {
                // Return 400 Bad Request for expected errors
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Return 500 Internal Server Error for unexpected errors
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }


        }

        [HttpGet("get-shared-resources-for-student")]
        public async Task<IActionResult> GetSharedResourcesForStudent([FromQuery] string email, [FromQuery] int packageId)
        {
            if (string.IsNullOrEmpty(email) || packageId <= 0)
            {
                return BadRequest(new { message = "Email and PackageId are required." });
            }

            try
            {
                // Get shared resources for the student and package
                var sharedResources = await _UserAppService.GetSharedResourcesForStudentAsync(email, packageId);

                if (sharedResources == null || !sharedResources.Any())
                {
                    return NotFound(new { message = "No shared resources found for the specified package and user." });
                }

                return Ok(sharedResources);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("get-all-packages")]
        public async Task<IActionResult> GetAllPackages()
        {
            try
            {
                var packages = await _UserAppService.GetAllPackagesAsync();

                if (packages == null || !packages.Any())
                {
                    return NotFound(new { message = "No packages found." });
                }

                return Ok(packages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("get-all-coaches")]
        public async Task<IActionResult> GetAllCoaches()
        {
            try
            {
                var coaches = await _UserAppService.GetAllCoachesAsync();

                if (coaches == null || !coaches.Any())
                {
                    return NotFound(new { message = "No Coaches found." });
                }

                return Ok(coaches);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

     

  

        [HttpGet("download-questions-document")]
        public async Task<IActionResult> DownloadQuestionsDocument([FromQuery] int testId)
        {
            if (testId <= 0)
            {
                return BadRequest(new { message = "Valid test ID is required." });
            }

            try
            {
                // Test bilgilerini al
                var test = await _UserAppService.GetTestByIdAsync(testId);

                if (test == null || string.IsNullOrEmpty(test.QuestionsDocument))
                {
                    return NotFound(new { message = "Questions document not found for this test." });
                }

                // Base64'ü byte array'e dönüştür
                var documentBytes = Convert.FromBase64String(test.QuestionsDocument);

                // Tarayıcıya dosyayı indirme talimatı ver
                return File(documentBytes, "application/pdf", $"{test.TestName}_Questions.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        

        [HttpPost("create-work-schedule")]
        public async Task<IActionResult> CreateWorkSchedule([FromBody] WorkScheduleDTO workScheduleDTO)
        {
            if (workScheduleDTO == null || string.IsNullOrEmpty(workScheduleDTO.Email))
            {
                return BadRequest(new { message = "Email and GeneralNotes are required." });
            }

            try
            {
                var createdSchedule = await _UserAppService.CreateWorkScheduleAsync(workScheduleDTO);
                return Ok(new { message = "WorkSchedule created successfully.", data = createdSchedule });
            }
            catch (ArgumentException ex)
            {
                // Kullanıcı hataları için 400 döner
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Genel bir hata durumu için
                return StatusCode(500, new
                {
                    message = "An error occurred while creating the work schedule.",
                    details = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }

        [HttpGet("get-work-schedules")]
        public async Task<IActionResult> GetWorkSchedulesByEmail([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { message = "Email is required." });
            }

            try
            {
                var workSchedules = await _UserAppService.GetWorkSchedulesByEmailAsync(email);

                if (workSchedules == null || !workSchedules.Any())
                {
                    return NotFound(new { message = "No work schedules found for this user." });
                }

                return Ok(workSchedules);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }




    }
}
