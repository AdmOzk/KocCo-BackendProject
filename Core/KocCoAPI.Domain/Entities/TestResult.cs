using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KocCoAPI.Domain.Entities
{
    public class TestResult
    {
        [Key]
        public int TestResultId { get; set; }

        public int TestId { get; set; }

        public int StudentId { get; set; }

        public int Grade { get; set; }

    }
}
