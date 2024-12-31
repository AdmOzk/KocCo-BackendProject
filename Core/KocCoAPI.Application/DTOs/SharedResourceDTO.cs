using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KocCoAPI.Application.DTOs
{
    public class SharedResourceDTO
    {
        public string DocumentName { get; set; } // Dokümanın adı
        public string Document { get; set; }    // Dokümanın içeriği (Base64 veya Text)

    }
}
