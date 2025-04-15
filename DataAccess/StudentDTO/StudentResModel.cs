using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.StudentDTO
{
    public class StudentResModel
    {
        public string StudentCode { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string EnrollmentDate { get; set; } = null!;
    }
}
