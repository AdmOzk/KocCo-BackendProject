using System.ComponentModel.DataAnnotations;

namespace KocCoAPI.Domain.Entities
{
    public class CoachPackage
    {
        [Key]
        public int CoachId { get; set; }

        [Key]
        public int PackageId { get; set; }
    }
}
