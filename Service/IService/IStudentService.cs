using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.StudentDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Service.IService
{
    public interface IStudentService
    {
        Task<StudentResModel> VerifyStudent(StudentReqModel request);
        Task<string> SaveImage(IFormFile image);
    }
}
