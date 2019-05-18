using Microsoft.EntityFrameworkCore;
using PortolWeb.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortolWeb.DA
{
   public class DataContext : DbContext, IDataContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<CodeVerification> CodeVerifications { get; set; }
        public DbSet<Script> Scripts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<VehiculeType> VehiculeType { get; set; }
        public DbSet<VehiculeTypeRange> VehiculeTypeRange { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        
        }      
    }
}
