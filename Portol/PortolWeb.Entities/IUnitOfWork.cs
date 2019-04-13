using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortolWeb.Entities
{
    public interface IUnitOfWork
    {
        IRepositoryBasey<Customer> CustomerRepository { get; }
        IRepositoryBasey<Address> AddressRepository { get; }
        IRepositoryBasey<CodeVerification> CodeVerificationRepository { get; }

        IDbContextTransaction BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        void SaveChanges();
    }


}
