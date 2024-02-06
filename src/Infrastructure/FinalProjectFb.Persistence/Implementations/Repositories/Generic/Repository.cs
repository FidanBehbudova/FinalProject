﻿using FinalProjectFb.Application.Abstractions.Repositories.Generic;
using FinalProjectFb.Domain.Entities.Common;
using FinalProjectFb.Persistence.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Persistence.Implementations.Repositories.Generic
{
    public class Repository<T> : IRepository<T> where T : BaseEntity, new()
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IQueryable<T> GetAll(bool isTracking = false, bool ignoreQuery = false, params string[] includes)
        {
            IQueryable<T> query = _dbSet;
            if (ignoreQuery) query = query.IgnoreQueryFilters();
            if (!isTracking) query = query.AsNoTracking();
            query = _addIncludes(query, includes);

            return query;
        }
        public IQueryable<T> GetAllWhere(Expression<Func<T, bool>>? expression = null,
            bool IsTracking = false,
            params string[] includes)
        {

            IQueryable<T> query = _context.Set<T>();
            if (expression != null) query = query.Where(expression);
            query = _addIncludes(query, includes);

            return IsTracking ? query : query.AsNoTracking();

        }

        public async Task<bool> IsExist(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression);
        }


        public IQueryable<T> GetOrder(Expression<Func<T, object>>? orderExpression = null, bool isDescending = false)
        {
            IQueryable<T> query = _context.Set<T>();
            if (orderExpression != null)
            {
                if (isDescending)
                {
                    query = query.OrderByDescending(orderExpression);
                }
                else
                {
                    query = query.OrderBy(orderExpression);
                }

            }
            return query;
        }

        public IQueryable<T> GetPagination(int skip = 0, int take = 0, bool ignoreQuery = false, params string[] includes)
        {
            IQueryable<T> query = _context.Set<T>();
            if (skip != 0) query = query.Skip(skip);
            if (take != 0) query = query.Take(take);
            if (ignoreQuery) query = query.IgnoreQueryFilters();
            return query;
        }

        public async Task<T> GetByIdAsync(int id, bool isTracking = false, bool ignoreQuery = false, params string[] includes)
        {
            IQueryable<T> query = _dbSet.Where(x => x.Id == id);
            if (ignoreQuery) query = query.IgnoreQueryFilters();
            if (!isTracking) query = query.AsNoTracking();
            query = _addIncludes(query, includes);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<T> GetByExpressionAsync(Expression<Func<T, bool>> expression, int id, bool isTracking = false, bool ignoreQuery = false, params string[] includes)
        {
            IQueryable<T> query = _dbSet.Where(expression);
            if (ignoreQuery) query = query.IgnoreQueryFilters();
            if (!isTracking) query = query.AsNoTracking();
            query = _addIncludes(query, includes);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AnyAsync(expression);
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
        public void SoftDelete(T entity)
        {
            entity.IsDeleted = true;
        }
        public void ReverseDelete(T entity)
        {
            entity.IsDeleted = false;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        private IQueryable<T> _addIncludes(IQueryable<T> query, params string[] includes)
        {
            if (includes != null)
            {
                for (int i = 0; i < includes.Length; i++)
                {
                    query = query.Include(includes[i]);
                }
            }

            return query;
        }
    }
}
