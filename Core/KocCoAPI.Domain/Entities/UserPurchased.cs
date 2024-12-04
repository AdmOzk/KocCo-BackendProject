namespace KocCoAPI.Domain.Entities
{
    public class UserPurchased
    {
        public int UserPackageID { get; set; }
        public int StudentID { get; set; }
        public int PackageID { get; set; }

        public Package? Package { get; set; } // Package ile ilişki

        public User? User { get; set; } // User ile ilişki
    }
}
