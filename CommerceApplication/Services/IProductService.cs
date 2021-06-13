using CommerceApplication.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommerceApplication.Services
{
    public interface IProductService
    {
        Task<IReadOnlyCollection<Product>> GetAllProducts(string UserName);
        Task<Product> Product_Get(int productId);
        Task<bool> CreateProduct(Product product, IFormFile file, string UserName);
        Task<Product> EditProduct_Get(int productId, string userId);
        Task<bool> EditProduct(Product product, IFormFile file, string userId);
        Task<Product> DeleteProduct_Get(int productId, string userId);
        Task<bool> DeleteProduct(int productId, string userId);
    }
}
