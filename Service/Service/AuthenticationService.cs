﻿using System.Linq.Expressions;
using System.Net;
using AutoMapper;
using BusinessObjects.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DataAccess.PasswordDTO;
using DataAccess.StudentDTO;
using DataAccess.UserDTO;
using Service.IService;
using Service.Utils.CustomException;
using Service.Utils.Security;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJWTService _jwtService;
    private readonly IEmailService _emailService;
    private readonly IStudentService _studentService;
    private readonly Cloudinary _cloudinary;

    public AuthenticationService(IUnitOfWork unitOfWork, IJWTService jwtService, 
        IEmailService emailService, IStudentService studentService, Cloudinary cloudinary)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
        _emailService = emailService;
        _studentService = studentService;
        _cloudinary = cloudinary;
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


    public async Task<UserProfileModel> GetUserInfor(string token)
    {
        var userId = _jwtService.decodeToken(token, "userId");

        var user = await _unitOfWork.Users.GetByIdAsync(userId,
            includeProperties: new Expression<Func<User, object>>[] { u => u.Role, u => u.Student, u => u.Shop });

        if (user == null)
            throw new ApiException(HttpStatusCode.NotFound, "User not found.");

        var profile = new UserProfileModel
        {
            UserId = user.UserId,
            FullName = user.FullName,
            Email = user.Email,
            RoleName = user.Role.RoleName,
            Avatar = user.Avatar,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address,
            Dob = user.Dob,
            Gender = user.Gender,
            CreatedAt = user.CreatedAt
        };

        if (user.Role.RoleName == "Student" && user.Student != null)
        {
            profile.StudentCode = user.Student.StudentCode;
            profile.IdentityCard = user.Student.IdentityCard;
            profile.EnrollmentDate = user.Student.EnrollmentDate;
        }

        if (user.Role.RoleName == "Shop" && user.Shop != null) 
        {
            profile.ShopName = user.Shop.ShopName;
            profile.ShopPhone = user.Shop.ShopPhone;
            profile.ShopAddress = user.Shop.ShopAddress;
            profile.BusinessLicense = user.Shop.BusinessLicense;
            profile.BankName = user.Shop.BankName;
            profile.BankNumber = user.Shop.BankNumber;
        }
        return profile;
    }

    private string GenerateTemporaryPassword()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 10)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }


    public async Task RegisterStudent(StudentRegisterReqModel request)
    {
        var existingUser = await _unitOfWork.Users.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new ApiException(HttpStatusCode.BadRequest, "Email already registered.");
        }
        
        if (request.StudentCardImage == null)
        {
            throw new ApiException(HttpStatusCode.BadRequest, "Student card image is required.");
        }

        // Tạo đối tượng StudentReqModel từ StudentRegisterReqModel
        var studentRequest = new StudentReqModel
        {
            FullName = request.FullName,
            StudentCode = request.StudentCode,
            EnrollmentDate = request.EnrollmentDate,
            Image = request.StudentCardImage
        };

        // Xác thực thẻ sinh viên
        var studentVerification = await _studentService.VerifyStudent(studentRequest);
        if (studentVerification == null)
        {
            throw new ApiException(HttpStatusCode.BadRequest, "Student card verification failed. Cannot register.");
        }

        using var stream = request.AvatarImage.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(request.AvatarImage.FileName, stream),
            Folder = "user_avatars"
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        if (uploadResult.Error != null)
        {
            throw new ApiException(HttpStatusCode.InternalServerError, uploadResult.Error.Message);
        }

        string avatarUrl = uploadResult.SecureUrl.ToString();

        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            Password = PasswordHasher.HashPassword(request.Password),
            RoleId = 2, 
            CreatedAt = DateTime.UtcNow,
            Dob = request.Dob,
            Address = request.Address,
            PhoneNumber = request.PhoneNumber,
            Gender = request.Gender,
            Avatar = avatarUrl,
            Status = "Active"
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveAsync();

        var student = new Student
        {
            UserId = user.UserId,
            StudentCode = request.StudentCode,
            IdentityCard = request.IdentityCard,
            EnrollmentDate = request.EnrollmentDate
        };

        await _unitOfWork.Student.AddAsync(student);
        await _unitOfWork.SaveAsync();

    }

    public async Task Register(UserRegisterReqModel request)
    {
        var existingUser = await _unitOfWork.Users.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new ApiException(HttpStatusCode.BadRequest, "Email already registered.");
        }

        var role = await _unitOfWork.Role.FirstOrDefaultAsync(r => r.RoleId == request.RoleId);
        if (role == null)
        {
            throw new ApiException(HttpStatusCode.BadRequest, "Invalid role.");
        }

        if (request.AvatarImage == null || request.AvatarImage.Length == 0)
        {
            throw new ApiException(HttpStatusCode.BadRequest, "Avatar image is required.");
        }

        using var stream = request.AvatarImage.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(request.AvatarImage.FileName, stream),
            Folder = "user_avatars"
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        if (uploadResult.Error != null)
        {
            throw new ApiException(HttpStatusCode.InternalServerError, uploadResult.Error.Message);
        }

        string avatarUrl = uploadResult.SecureUrl.ToString();


        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            Password = PasswordHasher.HashPassword(request.Password),
            RoleId = request.RoleId,
            CreatedAt = DateTime.UtcNow,
            Dob = request.Dob,
            Address = request.Address,
            PhoneNumber = request.PhoneNumber,
            Gender = request.Gender,
            Avatar = avatarUrl,
            Status = "Active"
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveAsync();      
       
    }

    public async Task UpdateUserProfile(string token, UpdateProfileReqModel request)
    {
        var userId = _jwtService.decodeToken(token, "userId");


        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
            throw new ApiException(HttpStatusCode.NotFound, "User not found.");
        
        using var stream = request.AvatarImage.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(request.AvatarImage.FileName, stream),
            Folder = "user_avatars"
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        if (uploadResult.Error != null)
        {
            throw new ApiException(HttpStatusCode.InternalServerError, uploadResult.Error.Message);
        }

        string avatarUrl = uploadResult.SecureUrl.ToString();

        if (!string.IsNullOrWhiteSpace(request.Dob))
            user.Dob = request.Dob;

        if (!string.IsNullOrWhiteSpace(request.Address))
            user.Address = request.Address;

        if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            user.PhoneNumber = request.PhoneNumber;

        if (!string.IsNullOrWhiteSpace(request.Gender))
            user.Gender = request.Gender;

        if (!string.IsNullOrWhiteSpace(avatarUrl))
            user.Avatar = avatarUrl;


        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveAsync();
    }

    public async Task RegisterShop(ShopRegisterReqModel request)
    {
        var existingUser = await _unitOfWork.Users.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new ApiException(HttpStatusCode.BadRequest, "Email already registered.");
        }

        using var stream = request.AvatarImage.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(request.AvatarImage.FileName, stream),
            Folder = "user_avatars"
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        if (uploadResult.Error != null)
        {
            throw new ApiException(HttpStatusCode.InternalServerError, uploadResult.Error.Message);
        }

        string avatarUrl = uploadResult.SecureUrl.ToString();

        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            Password = PasswordHasher.HashPassword(request.Password),
            RoleId = 6,
            CreatedAt = DateTime.UtcNow,
            Dob = request.Dob,
            Address = request.Address,
            PhoneNumber = request.PhoneNumber,
            Gender = request.Gender,
            Avatar = avatarUrl,          
            Status = "Active"
        };
        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveAsync();

        var shop = new Shop
        {
            UserId = user.UserId,
            ShopName = request.ShopName,
            ShopAddress = request.ShopAddress,
            ShopPhone = request.ShopPhone,
            BankName = request.BankName,
            BankNumber = request.BankNumber,
            BusinessLicense = request.BusinessLicense,
            CreatedDate = DateTime.UtcNow,
            Status = "Active"
        };

        await _unitOfWork.Shop.AddAsync(shop);
        await _unitOfWork.SaveAsync();
    }
}
