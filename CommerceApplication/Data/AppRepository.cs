using CommerceApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
//using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CommerceApplication.Data
{
    public class AppRepository : IAppRepository
    {

        private ApplicationDbContext context;
        public AppRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Product> QueryinContext(Expression<Func<Product, bool>> filter = null,
            Func<IQueryable<Product>, IIncludableQueryable<Product, object>> include = null)
        {
            IQueryable<Product> query = context.Products.AsQueryable();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (include != null)
            {
                query = include(query);
            }

            return query;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(Func<IQueryable<Product>, IIncludableQueryable<Product, object>> include = null)
        {
            return await QueryinContext(include: include).ToListAsync();
        }

        public IEnumerable<Product> GetAllProducts(Func<IQueryable<Product>, IIncludableQueryable<Product, object>> include = null)
        {
            return QueryinContext(include: include).ToList();
        }

        public async Task<IEnumerable<Product>> GetProductAsync(Expression<Func<Product, bool>> filter,
            Func<IQueryable<Product>, IIncludableQueryable<Product, object>> include = null)
        {
            return await QueryinContext(filter: filter,
                include: include
                ).ToListAsync();
        }

        public IEnumerable<Product> GetProduct(Expression<Func<Product, bool>> filter,
            Func<IQueryable<Product>, IIncludableQueryable<Product, object>> include = null)
        {
            return QueryinContext(filter: filter,
                include: include
                ).ToList();
        }

        public Task<Product> GetOneProductAsync(Expression<Func<Product, bool>> filter,
            Func<IQueryable<Product>, IIncludableQueryable<Product, object>> include = null)
        {
            return QueryinContext(filter: filter,
                include: include
                ).FirstOrDefaultAsync();
        }

        public Product GetOneProduct(Expression<Func<Product, bool>> filter,
            Func<IQueryable<Product>, IIncludableQueryable<Product, object>> include = null)
        {
            return QueryinContext(filter: filter,
                include: include
                ).FirstOrDefault();
        }

        public bool GetExists(Expression<Func<Product, bool>> filter)
        {
            return QueryinContext(filter: filter).Any();
        }

        public Task<bool> GetExistsAsync(Expression<Func<Product, bool>> filter)
        {
            return QueryinContext(filter: filter).AnyAsync();
        }

        public void DeleteProduct(Product product)
        {
            context.Products.Remove(product);
        }
        public void UpdateProduct(Product product)
        {
            context.Set<Product>().Attach(product);
            context.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
        public void CreateProduct(Product product)
        {
            context.Set<Product>().Add(product);
        }

        public void SaveChanges()
        {
            try {
                context.SaveChanges();
            }
            catch(DbUpdateConcurrencyException ex)
            {
                throw;
            }
            
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw;
            }

        }

        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return context.Database.BeginTransactionAsync();
        }

        public void CommitTransaction()
        {
            context.Database.CommitTransaction();
        }
    }
}
