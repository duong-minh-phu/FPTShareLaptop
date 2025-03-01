using BusinessObjects.Models;
using Service.IService;

namespace Service.Service
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Sep490Context _context;
        private readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(Sep490Context context)
        {
            _context = context;
        }

        // Generic repository getter
        public IGenericRepository<T> Repository<T>() where T : class
        {
            if (_repositories.ContainsKey(typeof(T)))
                return (IGenericRepository<T>)_repositories[typeof(T)];

            var repository = new GenericRepository<T>(_context);
            _repositories.Add(typeof(T), repository);
            return repository;
        }

        // Specific repositories (for strongly-typed usage if needed)
        public IGenericRepository<Category> Categories => Repository<Category>();
        public IGenericRepository<Order> Orders => Repository<Order>();
        public IGenericRepository<OrderItem> OrderItems => Repository<OrderItem>();
        public IGenericRepository<Product> Products => Repository<Product>();
        public IGenericRepository<ProductImage> ProductImages => Repository<ProductImage>();
        public IGenericRepository<Role> Roles => Repository<Role>();
        public IGenericRepository<Shop> Shops => Repository<Shop>();
        public IGenericRepository<User> Users => Repository<User>();

        public IGenericRepository<BorrowDetail> BorrowDetail => Repository<BorrowDetail>();

        public IGenericRepository<BorrowHistory> BorrowHistory => Repository<BorrowHistory>();

        public IGenericRepository<BorrowRequest> BorrowRequest => Repository<BorrowRequest>();

        public IGenericRepository<CategoryItem> CategoryItem => Repository<CategoryItem>();

        public IGenericRepository<DonationForm> DonationForm => Repository<DonationForm>();

        public IGenericRepository<Feedback> Feedback => Repository<Feedback>();

        public IGenericRepository<Order> Order => Repository<Order>();

        public IGenericRepository<OrderItem> OrderItem => Repository<OrderItem>();

        public IGenericRepository<Product> Product => Repository<Product>();

        public IGenericRepository<ProductImage> ProductImage => Repository<ProductImage>();

        public IGenericRepository<Role> Role => Repository<Role>();

        public IGenericRepository<Shop> Shop => Repository<Shop>();

        public IGenericRepository<Sponsor> Sponsor => Repository<Sponsor>();

        public IGenericRepository<SponsorItem> SponsorItem => Repository<SponsorItem>();

        public IGenericRepository<SponsorItemImage> SponsorItemImage => Repository<SponsorItemImage>();

        public IGenericRepository<Student> Student => Repository<Student>();

        public IGenericRepository<RefreshToken> RefreshToken => Repository<RefreshToken>();

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
