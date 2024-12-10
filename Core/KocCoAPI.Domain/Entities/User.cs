namespace KocCoAPI.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
        public string Roles { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string PhoneNumber { get; set; }
        public string Skills { get; set; }
        public int? CartId { get; set; }

        public ICollection<UserPurchased>? UserPurchases { get; set; }

        public ICollection<TestStudent> TestStudents { get; set; }
    }
}
