using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Abstracts.Repositories;
using StoreApp.Domain.Entities;
using StoreApp.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity, new()
    {
        private StoreAppDbContext _context { get; }
        private DbSet<T> Table { get; }

        public Repository(StoreAppDbContext context)
        {
            _context = context;
            Table = _context.Set<T>();

        }

        public async Task AddAsync(T entity)
        {
            await Table.AddAsync(entity);
        }

        public void Update(T entity)
        {
            Table.Update(entity);
        }

        public void Delete(T entity)
        {
            Table.Remove(entity);
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await Table.FindAsync(id);
        }

        public IQueryable<T> GetByFiltered(Expression<Func<T, bool>>? predicate = null,
            Expression<Func<T, object>>[]? include = null,
            bool isTracking = false)
        {
            IQueryable<T> query = Table;


            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (include != null)
            {
                foreach (var includeExpression in include)
                {
                    query = query.Include(includeExpression);
                }
            }

            if (!isTracking)
            {
                query = query.AsNoTracking(); // Return the table without tracking changes
            }
            return query;
        }

        public IQueryable<T> GetAll(bool isTracking = false)
        {
            if (!isTracking)
                return Table.AsNoTracking(); // Return the table without tracking changes
            return Table;
        }

        public IQueryable<T> GetAllFiltered(Expression<Func<T, bool>>? predicate = null,
            Expression<Func<T, object>>[]? include = null,
            Expression<Func<T, bool>>? orderBy = null,
            bool isOrderByAsc = true,
            bool isTracking = false)
        {
            IQueryable<T> query = Table;
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (include != null)
            {
                foreach (var includeExpression in include)
                {
                    query = query.Include(includeExpression);
                }
            }

            if (orderBy != null)
            {
                query = isOrderByAsc ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
            }

            if (!isTracking)
            {
                query = query.AsNoTracking(); // Return the table without tracking changes
            }
            return query;
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<T>> GetByNameSearchAsync(string namePart, Expression<Func<T, string>> selector)
        {
            // selector (məs: x => x.Name) → "Name"
            string propertyName = GetPropertyName(selector);

            return await Table
                .Where(e => EF.Functions.Like(EF.Property<string>(e, propertyName), $"%{namePart}%"))
                .ToListAsync();
        }
        private string GetPropertyName(Expression<Func<T, string>> expression)
        {
            if (expression.Body is MemberExpression member)
                return member.Member.Name;

            if (expression.Body is UnaryExpression unary && unary.Operand is MemberExpression memberOperand)
                return memberOperand.Member.Name;

            throw new InvalidOperationException("Invalid selector expression");
        }

    }
}
