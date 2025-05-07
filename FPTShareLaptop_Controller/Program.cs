using System.Reflection;
using System.Text;
using BusinessObjects.Models;
using CloudinaryDotNet;
using FPTShareLaptop_Controller.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Service.IService;
using Service.Service;
using Service.Utils.MapperProfiles;
using DotNetEnv;
using Microsoft.Extensions.Logging;
var builder = WebApplication.CreateBuilder(args);


//-----------------------------------------DB-----------------------------------------
builder.Services.AddDbContext<Sep490Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.
// Thêm dịch vụ CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});
builder.Services.AddControllers();
//-----------------------------------------MIDDLEWARE---------------------------------------
builder.Services.AddSingleton<GlobalExceptionMiddleware>();
//-----------------------------------------SERVICES-----------------------------------------
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<OpenAIService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IDonateFormService, DonationFormService>();
builder.Services.AddScoped<IBorrowRequestService, BorrowRequestService>();
builder.Services.AddScoped<IItemConditionService, ItemConditionService>();
builder.Services.AddScoped<IBorrowContractService, BorrowContractService>();
builder.Services.AddScoped<IFeedbackBorrowService, FeedbackBorrowService>();
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPayOSService, PayOSService>();
builder.Services.AddScoped<IPaymentMethodService, PaymentMethodService>();
builder.Services.AddScoped<IShipmentService, ShipmentService>();
builder.Services.AddScoped<ITrackingInfoService, TrackingInfoService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IComputerVisionService, ComputerVisionService>();
builder.Services.AddScoped<ITransactionLogService, TransactionLogService>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddScoped<CloudinaryService>();
builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IOptions<CloudinarySettings>>().Value;
    return new Cloudinary(new Account(config.CloudName, config.ApiKey, config.ApiSecret));
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//-----------------------------------------AUTHENTICATION-----------------------------------------

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:JwtKey"])),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
        };
    });

//-----------------------------------------AUTHORIZATION-----------------------------------------
builder.Services.AddAuthorization();
//----------------------------------------------------------------------------------

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
     {
         {
            new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
            new string[] {}
         }
     });

});

builder.Services.AddLogging(options =>
{
    options.AddConsole(); // Log to console
    options.AddDebug();   // Log to Debug  
});


var app = builder.Build();
// Sử dụng CORS
app.UseCors("AllowAllOrigins");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
    
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

app.UseMiddleware<GlobalExceptionMiddleware>();
app.Run();
