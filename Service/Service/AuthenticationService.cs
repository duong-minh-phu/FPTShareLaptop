using System.Globalization;
using System.Linq.Expressions;
using System.Net;
using BusinessObjects.Models;
using DataAccess.PasswordDTO;
using DataAccess.UserDTO;
using Org.BouncyCastle.Crypto.Generators;
using Service.IService;
using Service.Service;
using Service.Utils.CustomException;
using Service.Utils.Security;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJWTService _jwtService;
    private readonly IEmailService _emailService;

    public AuthenticationService(IUnitOfWork unitOfWork, IJWTService jwtService, IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
        _emailService = emailService;
    }

    public async Task<UserLoginResModel> Login(UserLoginReqModel userLoginReqModel)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(userLoginReqModel.Email);

        if (user == null || !PasswordHasher.VerifyPassword(userLoginReqModel.Password, user.Password))
        {
            throw new ApiException(HttpStatusCode.Unauthorized, "Invalid email or password.");
        }

        var token = await _jwtService.GenerateJWT(user.UserId);
        var refreshToken = _jwtService.GenerateRefreshToken();

        // Tạo mới Refresh Token
        var newRefreshToken = new RefreshToken
        {
            UserId = user.UserId,
            Token = refreshToken,
            ExpiredAt = DateTime.UtcNow.AddDays(7), // Thời hạn refresh token
            CreatedAt = DateTime.UtcNow,
            Status = "Active"
        };

        // Lưu refresh token vào database
        await _unitOfWork.RefreshToken.AddAsync(newRefreshToken);
        await _unitOfWork.SaveAsync();

        return new UserLoginResModel
        {
            UserId = user.UserId,
            FullName = user.FullName,
            Email = user.Email,
            RoleName = user.Role.RoleName,
            ProfilePicture = user.ProfilePicture,
            Token = token,
            RefreshToken = refreshToken,
            TokenExpiration = newRefreshToken.ExpiredAt
        };
    }


    public async Task Logout(string refreshToken)
    {
        var refreshTokenEntry = await _unitOfWork.RefreshToken.GetByTokenAsync(refreshToken);

        if (refreshTokenEntry == null || refreshTokenEntry.User == null)
        {
            throw new ApiException(HttpStatusCode.NotFound, "Refresh token does not exist");
        }

        _unitOfWork.RefreshToken.Delete(refreshTokenEntry);
        await _unitOfWork.SaveAsync();
    }



    public async Task ChangePassword(string token, ChangePasswordReqModel changePasswordReqModel)
    {
        var userId = _jwtService.decodeToken(token, "userId");
        
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
            throw new ApiException(HttpStatusCode.NotFound, "User not found.");

        if (!PasswordHasher.VerifyPassword(changePasswordReqModel.OldPassword, user.Password))
            throw new ApiException(HttpStatusCode.Unauthorized, "Old password is incorrect.");

        user.Password = PasswordHasher.HashPassword(changePasswordReqModel.NewPassword);
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveAsync();
    }



    public async Task ForgotPassword(string email)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(email);

        if (user == null)
            throw new ApiException(HttpStatusCode.NotFound, "Email not found.");

        // Tạo mật khẩu mới tạm thời
        string newPassword = GenerateTemporaryPassword();

        // Mã hóa mật khẩu mới
        user.Password = PasswordHasher.HashPassword(newPassword);
        await _unitOfWork.SaveAsync();

        // Gửi email chứa mật khẩu mới
        await _emailService.SendUserResetPassword(user.FullName, user.Email, newPassword);

        Console.WriteLine($"Temporary password sent to {user.Email}");
    }


    public async Task<object> GetUserInfor(string token)
    {
        var userId = _jwtService.decodeToken(token, "userId");
        var user = await _unitOfWork.Users.GetByIdAsync(userId, includeProperties: new Expression<Func<User, object>>[] { u => u.Role, u => u.Student, u => u.Sponsor });

        if (user == null)
            throw new ApiException(HttpStatusCode.NotFound, "Email not found.");

        switch (user.Role?.RoleName)
        {
            case "Student":
                if (user.Student == null)
                    throw new Exception("Student profile not found.");

                return new StudentProfileModel
                {
                    UserId = user.UserId,
                    FullName = user.FullName,
                    Email = user.Email,
                    RoleName = user.Role.RoleName,
                    ProfilePicture = user.ProfilePicture,
                    Status = user.Status,
                    CreatedAt = user.CreatedAt,
                    StudentCode = user.Student.StudentCode,  // Sửa lỗi tại đây
                    DateOfBirth = user.Student.DateOfBirth,  // Sửa lỗi tại đây
                    EnrollmentDate = user.Student.EnrollmentDate  // Sửa lỗi tại đây
                };

            case "Sponsor":
                if (user.Sponsor == null)
                    throw new Exception("Sponsor profile not found.");

                return new SponsorProfileModel
                {
                    UserId = user.UserId,
                    FullName = user.FullName,
                    Email = user.Email,
                    RoleName = user.Role.RoleName,
                    ProfilePicture = user.ProfilePicture,
                    Status = user.Status,
                    CreatedAt = user.CreatedAt,
                    ContactPhone = user.Sponsor.ContactPhone,  // Sửa lỗi tại đây
                    ContactEmail = user.Sponsor.ContactEmail,  // Sửa lỗi tại đây
                    Address = user.Sponsor.Address  // Sửa lỗi tại đây
                };

            default:
                throw new Exception("Invalid user role.");
        }
    }

    private string GenerateTemporaryPassword()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 10)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }


    public async Task<UserRegisterResModel> RegisterStudent(StudentRegisterReqModel studentRegisterReqModel)
    {
        var existingUser = await _unitOfWork.Users.GetByEmailAsync(studentRegisterReqModel.Email);
        if (existingUser != null)
        {
            throw new ApiException(HttpStatusCode.BadRequest, "Email already registered.");
        }

        var role = await _unitOfWork.Role.GetAllAsync();
        var studentRole = role.FirstOrDefault(r => r.RoleName == "Student");
        if (studentRole == null)
        {
            throw new ApiException(HttpStatusCode.BadRequest, "Invalid role.");
        }

        var user = new User
        {
            FullName = studentRegisterReqModel.FullName,
            Email = studentRegisterReqModel.Email,
            Password = PasswordHasher.HashPassword(studentRegisterReqModel.Password),
            RoleId = studentRole.RoleId,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveAsync();

        var student = new Student
        {
            UserId = user.UserId,
            StudentCode = studentRegisterReqModel.StudentCode,
            DateOfBirth = DateOnly.ParseExact(studentRegisterReqModel.DateOfBirth, "dd/MM/yyyy", null),
            EnrollmentDate = DateOnly.ParseExact(studentRegisterReqModel.EnrollmentDate, "dd/MM/yyyy", null)
        };

        await _unitOfWork.Student.AddAsync(student);
        await _unitOfWork.SaveAsync();

        return new UserRegisterResModel
        {
            UserId = user.UserId,
            Email = user.Email,
            Role = studentRole.RoleName,
            Message = "Student registered successfully."
        };
    }

    public async Task<UserRegisterResModel> RegisterSponsor(SponsorRegisterReqModel sponsorRegisterReqModel)
    {
        var existingUser = await _unitOfWork.Users.GetByEmailAsync(sponsorRegisterReqModel.Email);
        if (existingUser != null)
        {
            throw new ApiException(HttpStatusCode.BadRequest, "Email already registered.");
        }

        var role = await _unitOfWork.Role.GetAllAsync();
        var sponsorRole = role.FirstOrDefault(r => r.RoleName == "Sponsor");
        if (sponsorRole == null)
        {
            throw new ApiException(HttpStatusCode.BadRequest, "Invalid role.");
        }

        var user = new User
        {
            FullName = sponsorRegisterReqModel.FullName,
            Email = sponsorRegisterReqModel.Email,
            Password = PasswordHasher.HashPassword(sponsorRegisterReqModel.Password),
            RoleId = sponsorRole.RoleId,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveAsync();

        var sponsor = new Sponsor
        {
            UserId = user.UserId,
            ContactPhone = sponsorRegisterReqModel.ContactPhone,
            ContactEmail = sponsorRegisterReqModel.ContactEmail,
            Address = sponsorRegisterReqModel.Address
        };

        await _unitOfWork.Sponsor.AddAsync(sponsor);
        await _unitOfWork.SaveAsync();

        return new UserRegisterResModel
        {
            UserId = user.UserId,
            Email = user.Email,
            Role = sponsorRole.RoleName,
            Message = "Sponsor registered successfully."
        };
    }
}
