using Microsoft.EntityFrameworkCore;
using PortolWeb.Services.DBContext.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortolWeb.Services.DBContext
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Business> Businesses { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<User>()
              .HasKey(p => new { p.UserID });
        }

    }
}
