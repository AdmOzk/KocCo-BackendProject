namespace KocCoAPI.Domain.Entities
{
    public class CoachingSession
    {
        public int SessionID { get; set; }
        public int StudentID { get; set; }
        public int CoachID { get; set; }
        public DateTime SessionDate { get; set; }
        public string SessionNotes { get; set; }
        public decimal? Rating { get; set; }
    }
}
