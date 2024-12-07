using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KocCoAPI.Application.DTOs
{
    public class CardPackageDTO
    {
        public int CartId { get; set; }
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
