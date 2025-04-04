﻿using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Service.IService;
using System.Linq.Expressions;

namespace Service.Service
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly Sep490Context _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(Sep490Context context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync(
    Expression<Func<T, bool>> filter = null,
    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
    params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(object id, params Expression<Func<T, object>>[] includeProperties)
        {
            var keyName = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.First().Name;

            IQueryable<T> query = _dbSet;

            // Bao gồm các thuộc tính liên quan nếu có
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            // Sử dụng tên khóa chính chính xác
            return await query.FirstOrDefaultAsync(e => EF.Property<object>(e, keyName).Equals(id));
        
         }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Role)  // Eager load Role
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<RefreshToken> GetByTokenAsync(string refreshToken)
        {
            return await _context.Set<RefreshToken>()
                .Include(rt => rt.User) // Lấy luôn User
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
    }
}
