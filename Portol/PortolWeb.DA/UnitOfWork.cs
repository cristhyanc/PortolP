using Microsoft.EntityFrameworkCore.Storage;
using PortolWeb.DA.Repositories;
using PortolWeb.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortolWeb.DA
{
   public  class UnitOfWork: IUnitOfWork
    {
        private readonly DataContext _context;
        private IRepositoryBasey<Customer> _customerRepository;
        private IRepositoryBasey<CodeVerification> _codeVerificationRepository;
        private IRepositoryBasey<Address> _addressRepository;

        public UnitOfWork(DataContext context)
        {
            _context = context;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
            
        }


        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
             _context.Database.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            _context.Database.RollbackTransaction();
        }

        public IDataContext Context
        {
            get
            {
                return _context;
            }
        }

        public IRepositoryBasey<Address> AddressRepository
        {
            get
            {
                return _addressRepository = _addressRepository ?? new RepositoryBase<Address>(_context);
            }
        }

        public IRepositoryBasey<Customer> CustomerRepository
        {
            get
            {
                return _customerRepository = _customerRepository ?? new RepositoryBase<Customer>(_context);
            }
        }

        public IRepositoryBasey<CodeVerification> CodeVerificationRepository
        {
            get
            {
                return _codeVerificationRepository = _codeVerificationRepository ?? new RepositoryBase<CodeVerification>(_context);
            }
        }

    }
}
