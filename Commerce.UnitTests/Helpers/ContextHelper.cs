using CommerceApplication.Controllers;
using CommerceApplication.Data;
using CommerceApplication.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commerce.UnitTests.Helpers
{
    public static class ContextHelper
    {
        public static string CreateDbName() => $"Commerce_" + Guid.NewGuid();

        public static ApplicationDbContext CreateContext(string DbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(DbName)
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            return new ApplicationDbContext(options);
        }

        public static ProductService CreateProductService(
            ApplicationDbContext dbContext)
        {
            IAppRepository appRepository = CreateRepository(dbContext);
            var store = new UserStore<IdentityUser>(dbContext);
            UserManager<IdentityUser>  mockUser = new UserManager<IdentityUser>(store, null, null, null, null, null, null, null, null);
            var list = dbContext.Users.ToList();
            mockUser.GetRolesAsync(It.IsAny<IdentityUser>());
            mockUser.AddToRoleAsync(list[0], "Admin");
            ILogger<ProductService> logger = Mock.Of<ILogger<ProductService>>();
            return new ProductService(appRepository, logger, mockUser);
        }
        private static async Task<IList<string>> get()
        {
            List<IList<string>> allFaqs = new List<IList<string>>();
            IList<string> expected = new List<string>() { "Admin" };
            return expected;
        }
        public static Mock<UserManager<TUser>> MockUserManager<TUser>(List<TUser> ls) where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

            mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<TUser, string>((x, y) => ls.Add(x));
            mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);

            return mgr;
        }

        public static AppRepository CreateRepository(ApplicationDbContext dbContext)
        {
            return new AppRepository(dbContext);
        }
    }
}
