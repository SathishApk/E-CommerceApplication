using AutoMapper;
using CommerceApplication.Data;
using CommerceApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CommerceApplication.Services
{
    public class ProductService : IProductService
    {
        private readonly IAppRepository _appDbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<ProductService> _logger;
        public ProductService(IAppRepository appDbContext, ILogger<ProductService> logger,UserManager<IdentityUser> userManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IReadOnlyCollection<Product>> GetAllProducts(string UserName)
        {
            _logger.LogInformation($"GetAllProducts Api Called by {UserName}");
            var user = _userManager.Users.FirstOrDefault(x => x.Email.Equals(UserName));
            var RolesForUser = await _userManager.GetRolesAsync(user);
            IEnumerable<Product> products = new List<Product>();

            // Check If User Exists and has Admin Role 
            // Admin Users can see their own Products alone and have the ability to update 
            if (user != null && RolesForUser.Contains("Admin"))
            {
                products = await _appDbContext.GetProductAsync(x => x.Owner.Equals(user.Id));
            }
            else
            {
                // Users (Role = Users) can see all the Products in the Inventory
                products = await _appDbContext.GetAllProductsAsync();
            }
            return products.ToList();

        }

        public async Task<bool> CreateProduct(Product product, IFormFile file, string userName)
        {
            product.ProductImageData = ImageConversion(file);
            
            //Create a Product if The Image Data exists or it is Valid
            if (product.ProductImageData != null && _userManager.Users.Any(x => x.Email.Equals(userName)))
            {
                try
                {
                    using (var transaction = await _appDbContext.BeginTransactionAsync())
                    {
                        product.CreatedDttm = DateTime.Now;
                        product.CreatedBy = _userManager.Users.First(x => x.Email.Equals(userName)).Id;
                        product.Owner = _userManager.Users.First(x => x.Email.Equals(userName)).Id;
                        _appDbContext.CreateProduct(product);
                        await _appDbContext.SaveChangesAsync();
                        transaction.Commit();
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    return false;
                }                
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Product> EditProduct_Get(int productId, string userId)
        {
            return await _appDbContext.GetOneProductAsync(x => x.ProductId.Equals(productId) && x.Owner.Equals(userId));
        }

        public async Task<Product> Product_Get(int productId)
        {
            return await _appDbContext.GetOneProductAsync(x => x.ProductId.Equals(productId));
        }

        public async Task<bool> EditProduct(Product product, IFormFile file,string userId)
        {
            var dbProduct = _appDbContext.GetOneProduct(x => x.ProductId == product.ProductId && x.Owner.Equals(userId));
            
            // Edit a Product only if it exists in the DB and update only the Columns necessary from the arrived Model
            // To prevent unintended Updates
            if (dbProduct != null)
            {
                try
                {
                    dbProduct.ProductName = product.ProductName;
                    dbProduct.Price = product.Price;
                    dbProduct.ProductDescription = product.ProductDescription;
                    dbProduct.Category = product.Category;
                    Byte[] imageData = null;
                    if (file != null)
                    {
                        imageData = ImageConversion(file);
                    }
                    if (imageData != null && imageData.Length > 0)
                    {
                        dbProduct.ProductImageData = imageData;
                    }
                    dbProduct.LastUpdateDttm = DateTime.Now;
                    using (var transaction = await _appDbContext.BeginTransactionAsync())
                    {
                        _appDbContext.UpdateProduct(dbProduct);
                        await _appDbContext.SaveChangesAsync();
                        transaction.Commit();
                    }
                    return true;
                }
                catch(Exception e)
                {
                    _logger.LogError(e.Message);
                    return false;
                }
            }
            else
            {
                return false;
            }
            
        }

        public async Task<Product> DeleteProduct_Get(int productId, string userId)
        {
            return await _appDbContext.GetOneProductAsync(x => x.ProductId == productId && x.Owner.Equals(userId));
        }

        public async Task<bool> DeleteProduct(int productId, string userId)
        {
            var product = await _appDbContext.GetOneProductAsync(x => x.ProductId == productId && x.Owner.Equals(userId));
            // Delete a Product if it exists in DB
            if (product != null)
            {
                try
                {
                    using (var transaction = await _appDbContext.BeginTransactionAsync())
                    {
                        _appDbContext.DeleteProduct(product);
                        await _appDbContext.SaveChangesAsync();
                        transaction.Commit();
                    }
                    return true;
                }
                catch(Exception e)
                {
                    _logger.LogError(e.Message);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        // Convert Image into Byte Array
        public Byte[] ImageConversion(IFormFile file)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                
                file.CopyTo(ms);
                byte[] productImageData = ms.ToArray();
                ms.Close();
                ms.Dispose();
                if (IsValidImage(ms.ToArray()))
                {
                    return productImageData;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return null;
            }
        }

        // Check if the Image is Valid
        public bool IsValidImage(Byte[] imageData)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(imageData))
                    Image.FromStream(ms);
            }
            catch (ArgumentException e)
            {
                _logger.LogError(e.Message);
                return false;
            }
            return true;
        }
    }
}
