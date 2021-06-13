using CommerceApplication.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommerceApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public virtual DbSet<Product> Products {get;set;}
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>()
                .ToTable("Products", "Ecommerce")
                .HasKey(x=>x.ProductId);
        }
    }
}
