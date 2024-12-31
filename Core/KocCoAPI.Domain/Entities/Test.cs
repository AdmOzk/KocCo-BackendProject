namespace KocCoAPI.Domain.Entities
{
    public class Test
    {
        public int TestId { get; set; }
        public string TestName { get; set; }
        public string QuestionsDocument { get; set; }

        public ICollection<TestStudent> TestStudents { get; set; }
    }
}
