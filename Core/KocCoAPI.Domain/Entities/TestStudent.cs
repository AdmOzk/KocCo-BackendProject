namespace KocCoAPI.Domain.Entities
{
    public class TestStudent
    {
        public int TestId { get; set; }
        public int StudentId { get; set; }

        public Test Test { get; set; }
        public User User { get; set; } // Users tablosu ile ilişki
    }
}
