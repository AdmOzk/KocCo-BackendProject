namespace KocCoAPI.Domain.Entities
{
    public class SharedResource
    {
        public int SharedResourceId { get; set; }
        public int PackageId { get; set; }
        public string Document { get; set; }

        public string DocumentName { get; set; }
    }
}
