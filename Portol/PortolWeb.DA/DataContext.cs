using Microsoft.EntityFrameworkCore;
using PortolWeb.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortolWeb.DA
{
   public class DataContext : DbContext, IDataContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<CodeVerification> CodeVerifications { get; set; }
        public DbSet<Script> Scripts { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        
        }      
    }
}
