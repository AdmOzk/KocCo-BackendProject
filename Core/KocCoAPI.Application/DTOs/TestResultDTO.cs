using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KocCoAPI.Application.DTOs
{
    public class TestResultDTO
    {
        public int TestResultId { get; set; }

        public int TestId { get; set; }

        public int StudentId { get; set; }

        public int Grade { get; set; }


    }
}
