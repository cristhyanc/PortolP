﻿using Microsoft.EntityFrameworkCore;
using PortolWeb.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortolWeb.DA
{
   public class DataContext : DbContext, IDataContext
    {
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Vehicule> Vehicules { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Parcel> Parcels { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<CodeVerification> CodeVerifications { get; set; }
        public DbSet<Script> Scripts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<VehiculeType> VehiculeTypes { get; set; }
        public DbSet<VehiculeTypeRange> VehiculeTypeRanges { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        
        }      
    }
}
