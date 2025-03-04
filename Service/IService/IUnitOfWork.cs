using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<BorrowDetail> BorrowDetail { get; }
        IGenericRepository<BorrowHistory> BorrowHistory { get; }
        IGenericRepository<BorrowRequest> BorrowRequest { get; }
        IGenericRepository<Category> Categories { get; }
        IGenericRepository<CategoryItem> CategoryItem { get; }
        IGenericRepository<DonationForm> DonationForm { get; }
        IGenericRepository<Feedback> Feedback { get; }

        IGenericRepository<Order> Order { get; }
        IGenericRepository<OrderItem> OrderItem { get; }
        IGenericRepository<Product> Product { get; }
        IGenericRepository<ProductImage> ProductImage { get; }
        IGenericRepository<Role> Role { get; }
        IGenericRepository<Shop> Shop { get; }
        IGenericRepository<Sponsor> Sponsor { get; }
        IGenericRepository<SponsorItem> SponsorItem { get; }
        IGenericRepository<SponsorItemImage> SponsorItemImage { get; }
        IGenericRepository<Student> Student { get; }
        IGenericRepository<User> Users { get; }
        IGenericRepository<RefreshToken> RefreshToken { get; }

        Task<int> SaveAsync();
    }
}
