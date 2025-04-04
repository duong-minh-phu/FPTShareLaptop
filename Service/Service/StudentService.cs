using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DataAccess.StudentDTO;
using Microsoft.AspNetCore.Http;
using Service.IService;
using Service.Utils.CustomException;

namespace Service.Service
{
    public class StudentService : IStudentService
    {
        private readonly IComputerVisionService _computerVisionService;
        private readonly IJWTService _jwtService;
        private readonly IUnitOfWork _unitOfWork;

        public StudentService(IJWTService jwtService, IUnitOfWork unitOfWork, IComputerVisionService computerVisionService)
        {
            _computerVisionService = computerVisionService;
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> SaveImage(IFormFile image)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, $"{Guid.NewGuid()}_{image.FileName}");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return filePath;
        }

        public async Task<StudentResModel> VerifyStudent(StudentReqModel request)
        {
                     
            if (request.Image == null)
            {
                return null;
            }

            // Lưu ảnh tạm thời
            var imagePath = await SaveImage(request.Image);

            // Trích xuất văn bản từ ảnh
            var extractedText = await _computerVisionService.ExtractTextFromImageAsync(imagePath);

            // Làm sạch văn bản trích xuất
            string cleanedText = CleanExtractedText(extractedText);

            // Kiểm tra xác thực
            bool isVerified = CompareText(cleanedText, request.FullName, request.StudentCode);

            if (!isVerified)
            {
                return null; // Trả về null nếu thông tin không khớp
            }

            // Nếu xác thực thành công, trả về thông tin sinh viên
            return new StudentResModel
            {
                StudentCode = request.StudentCode,
                FullName = request.FullName,
                EnrollmentDate = request.EnrollmentDate
            };
        }

        private string CleanExtractedText(string text)
        {
            return text.ToLower()
                       .Replace("\n", " ")
                       .Replace("\r", " ")
                       .Trim();
        }

        private bool CompareText(string extractedText, string name, string studentCode)
        {
            var wordsInName = name.ToLower().Split(' ');
            var wordsInCode = studentCode.ToLower().Split(' ');

            return wordsInName.All(word => extractedText.Contains(word)) &&
                   wordsInCode.All(word => extractedText.Contains(word));
        }
    }
}
