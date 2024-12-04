namespace KocCoAPI.Domain.Entities
{
    public class TimeSlot
    {
        public int TimeSlotID { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int DayId { get; set; }
        public string Notes { get; set; }
    }
}
