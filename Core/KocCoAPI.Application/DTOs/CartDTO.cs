using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KocCoAPI.Application.DTOs
{
    public class CartDTO
    {
        public int CartId { get; set; }
        public int PackageId { get; set; } // Mapped from CartPackage.PackageId
        public string PackageName { get; set; } // Retrieved via join with Package
        public decimal TotalPrice { get; set; } // Computed by summing package prices in the cart

        public decimal Price { get; set; } // Add this property

    }
}
