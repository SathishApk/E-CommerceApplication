using Commerce.UnitTests.Helpers;
using CommerceApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commerce.UnitTests
{
    public class ProductCreationServiceTests
    {
        private string DbName;
        private IFormFile file;
        private string UserId;
        [SetUp]
        public void Setup()
        {
            DbName = ContextHelper.CreateDbName();
            using (var context = ContextHelper.CreateContext(DbName))
            {
                var roleList = new List<IdentityRole>()
                    {
                        new IdentityRole { Name = "Admin" },
                        new IdentityRole { Name = "User" },
                    };
                context.Roles.AddRange(roleList);
                context.SaveChanges();

                context.Users.Add(new IdentityUser { Email = "sathish@gmail.com", UserName = "sathish@gmail.com" });
                context.Users.Add(new IdentityUser { Email = "sathish123@gmail.com", UserName = "sathish123@gmail.com" });
                context.Users.Add(new IdentityUser { Email = "sathishUser@gmail.com", UserName = "sathishUser@gmail.com" });
                context.SaveChanges();

                var userRole = new IdentityUserRole<string>();
                userRole.RoleId = context.Roles.AsQueryable().First(x => x.Name.Equals("Admin")).Id;
                userRole.UserId = context.Users.AsQueryable().First(x => x.Email.Equals("sathish@gmail.com")).Id;
                UserId = userRole.UserId;

                var userRole_2 = new IdentityUserRole<string>();
                userRole_2.RoleId = context.Roles.AsQueryable().First(x => x.Name.Equals("Admin")).Id;
                userRole_2.UserId = context.Users.AsQueryable().First(x => x.Email.Equals("sathish123@gmail.com")).Id;

                var userRole_User = new IdentityUserRole<string>();
                userRole_User.RoleId = context.Roles.AsQueryable().First(x => x.Name.Equals("User")).Id;
                userRole_User.UserId = context.Users.AsQueryable().First(x => x.Email.Equals("sathishUser@gmail.com")).Id;

                context.UserRoles.Add(userRole);
                context.UserRoles.Add(userRole_2);
                context.UserRoles.Add(userRole_User);
                context.SaveChanges();

                var stream = File.OpenRead(@"C:\Users\91979\Pictures\Planet9\Planet9_3840x2160.jpg");

                file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(@"C:\Users\91979\Pictures\Planet9\Planet9_3840x2160.jpg"))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg"
                };
            }
        }

        [Test]
        public async Task CreateProductTest_SuccessSenario()
        {
            var actual = false;
            using (var context = ContextHelper.CreateContext(DbName))
            {
                var productService = ContextHelper.CreateProductService(context);
                Product product = new Product();
                product.Price = 20;
                product.ProductName = "Bikes";
                var IsSuccess = await productService.CreateProduct(product, file, "sathish@gmail.com");
                if(IsSuccess)
                {
                    var result = context.Products.Where(x => x.Owner.Equals(UserId)).FirstOrDefault();
                    if (result != null && result.Price == 20 && result.ProductName == "Bikes")
                    {
                        actual = true;
                    }
                }
            }
            Assert.AreEqual(true, actual);
        }

        [Test]
        public async Task CreateProductTest_FailSenario()
        {
            var actual = false;
            using (var context = ContextHelper.CreateContext(DbName))
            {
                var productService = ContextHelper.CreateProductService(context);
                Product product = new Product();
                product.Price = 20;
                product.ProductName = "Bikes";
                var IsSuccess = await productService.CreateProduct(product, file, "sathish1@gmail.com");
                if (IsSuccess) {}
                else
                {
                    actual = true;
                }
            }
            Assert.AreEqual(true, actual);
        }
    }
}
