namespace KocCoAPI.Domain.Entities
{
    public class WorkSchedule
    {
        public int WorkScheduleId { get; set; }
        
        public int? StudentId { get; set; }
       
        public string GeneralNotes { get; set; }
    }
}
