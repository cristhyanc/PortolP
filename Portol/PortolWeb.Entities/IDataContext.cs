using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortolWeb.Entities
{
    public interface IDataContext
    {

        DbSet<Driver> Drivers { get; set; }
        DbSet<Vehicule> Vehicules { get; set; }
        DbSet<Delivery> Deliveries { get; set; }
        DbSet<PaymentMethod> PaymentMethods { get; set; }
        DbSet<Parcel> Parcels { get; set; }
        DbSet<Picture> Pictures { get; set; }

        DbSet<Customer> Customers { get; set; }
        DbSet<Address> Addresses { get; set; }
        DbSet<CodeVerification> CodeVerifications { get; set; }
        DbSet<Business> Businesses { get; set; }
        DbSet<Script> Scripts { get; set; }
        DbSet<VehiculeType> VehiculeTypes { get; set; }
        DbSet<VehiculeTypeRange> VehiculeTypeRanges { get; set; }
        int SaveChanges();
        DatabaseFacade Database { get; }
    }
}
