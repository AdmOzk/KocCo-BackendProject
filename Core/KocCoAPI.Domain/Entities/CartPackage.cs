namespace KocCoAPI.Domain.Entities
{
    public class CartPackage
    {
        public int CartId { get; set; }
        public int PackageId { get; set; }

        public Cart Cart { get; set; }
        public Package Package { get; set; }
    }
}
