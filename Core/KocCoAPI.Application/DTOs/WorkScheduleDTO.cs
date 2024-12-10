using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KocCoAPI.Application.DTOs
{
    public class WorkScheduleDTO
    {
        public string Email { get; set; } // Email to fetch the UserId
        public string GeneralNotes { get; set; } // Notes for the schedule
    }
}
