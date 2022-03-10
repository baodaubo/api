using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace api.Models
{
    public class EntityDBContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(@"Server=PCITSW2\SQLEXPRESS;Database=dbapi;Trusted_Connection=True;");

         public DbSet<Product> Products { get; set; }
    }

}