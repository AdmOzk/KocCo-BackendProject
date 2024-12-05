using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KocCoAPI.Application.DTOs
{
    public class PackageDTO
    {
        public int PackageID { get; set; }
        public string PackageName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int DurationInDays { get; set; }
        //public DateTime CreatedAt { get; set; }
       // public DateTime? UpdatedAt { get; set; }
        public decimal? Rating { get; set; }
    }
}
