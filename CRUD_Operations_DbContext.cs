using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ICTBD_CRUD_Project.Models;
namespace ICTBD_CRUD_Project
{
    public class CRUD_Operations_DbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=BankingManagementSystem;Trusted_Connection=true;Encrypt=false;MultipleActiveResultSets=true");
        }
    }
}