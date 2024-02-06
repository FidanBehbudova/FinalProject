using FinalProjectFb.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Application.Abstractions.Repositories.Generic
{
    public interface IRepository<T> where T : BaseEntity, new()
    {
        IQueryable<T> GetAll(bool isTracking = false, bool ignoreQuery = false, params string[] includes);
        IQueryable<T> GetAllWhere(Expression<Func<T, bool>>? expression = null, bool isTracking = false, params string[] includes);
        IQueryable<T> GetOrder(Expression<Func<T, object>>? orderExpression = null, bool isDescending = false);
        IQueryable<T> GetPagination(int skip = 0, int take = 0, bool ignoreQuery = false, params string[] includes);
        Task<T> GetByIdAsync(int id, bool isTracking = false, bool ignoreQuery = false, params string[] includes);
        Task<T> GetByExpressionAsync(Expression<Func<T, bool>> expression, int id, bool isTracking = false, bool ignoreQuery = false, params string[] includes);
        Task<bool> IsExistAsync(Expression<Func<T, bool>> expression);
        Task<bool> IsExist(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        void SoftDelete(T entity);
        void ReverseDelete(T entity);
        Task SaveChangesAsync();
    }
}
