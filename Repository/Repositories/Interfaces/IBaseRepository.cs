﻿using Domain.Common;
using System.Linq.Expressions;

namespace Repository.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntity
    {

        IQueryable<T> GetFiltered(Expression<Func<T, bool>> expression, params string[] includes);
        Task<T?> GetSingleAsync(Expression<Func<T, bool>> expression, params string[] includes);
        Task<bool> IsExistAsync(Expression<Func<T, bool>> expression, params string[] includes);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task DeleteAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<int> SaveAsync();

    }
}
