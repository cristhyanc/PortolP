﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortolWeb.Entities
{
    public interface IDataContext
    {
        DbSet<Customer> Customers { get; set; }
        DbSet<Address> Addresses { get; set; }
        DbSet<CodeVerification> CodeVerifications { get; set; }
        DbSet<Business> Businesses { get; set; }
        DbSet<Script> Scripts { get; set; }
        int SaveChanges();
        DatabaseFacade Database { get; }
    }
}
