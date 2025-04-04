using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DataAccess.StudentDTO
{
    public class StudentReqModel
    {
        public string StudentCode { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string EnrollmentDate { get; set; } = null!;
        public IFormFile Image { get; set; }
    }
}
