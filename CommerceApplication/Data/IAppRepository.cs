using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using CommerceApplication.Models;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;

namespace CommerceApplication.Data
{
    public interface IAppRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(Func<IQueryable<Product>, IIncludableQueryable<Product, object>> include = null);
        IEnumerable<Product> GetAllProducts(Func<IQueryable<Product>, IIncludableQueryable<Product, object>> include = null);
        Task<IEnumerable<Product>> GetProductAsync(Expression<Func<Product, bool>> filter, Func<IQueryable<Product>, IIncludableQueryable<Product, object>> include = null);
        IEnumerable<Product> GetProduct(Expression<Func<Product, bool>> filter, Func<IQueryable<Product>, IIncludableQueryable<Product, object>> include = null);
        Task<Product> GetOneProductAsync(Expression<Func<Product, bool>> filter, Func<IQueryable<Product>, IIncludableQueryable<Product, object>> include = null);
        Product GetOneProduct(Expression<Func<Product, bool>> filter, Func<IQueryable<Product>, IIncludableQueryable<Product, object>> include = null);
        bool GetExists(Expression<Func<Product, bool>> filter);
        Task<bool> GetExistsAsync(Expression<Func<Product, bool>> filter);
        void DeleteProduct(Product product);
        void UpdateProduct(Product product);
        void CreateProduct(Product product);
        void SaveChanges();
        Task<int> SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
        void CommitTransaction();
    }
}
