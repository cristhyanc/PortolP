using Microsoft.EntityFrameworkCore.Storage;
using PortolWeb.DA.Repositories;
using PortolWeb.Entities;
using PortolWeb.Entities.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortolWeb.DA
{
   public  class UnitOfWork: IUnitOfWork
    {
        private readonly DataContext _context;
        private IRepositoryBase<Customer> _customerRepository;
        private IRepositoryBase<CodeVerification> _codeVerificationRepository;
        private IRepositoryBase<Address> _addressRepository;
        private IRepositoryBase<VehiculeTypeRange> _vehiculeTypeRangeRepository;
        private IVehiculeTypeRepository _vehiculeTypeRepository;
        private IRepositoryBase<Picture> _pictureRepository;
        private IDeliveryRepository _deliveryRepository;
        private IParcelRepository _parcelRepository;

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

        public IParcelRepository ParcelRepository
        {
            get
            {
                return _parcelRepository = _parcelRepository ?? new ParcelRepository(_context);
            }
        }

        public IDeliveryRepository DeliveryRepository
        {
            get
            {
                return _deliveryRepository = _deliveryRepository ?? new DeliveryRepository(_context);
            }
        }

        public IVehiculeTypeRepository VehiculeTypeRepository
        {
            get
            {
                return _vehiculeTypeRepository = _vehiculeTypeRepository ?? new VehiculeTypeRepository(_context);
            }
        }
                
        public IRepositoryBase<Picture> PictureRepository
        {
            get
            {
                return _pictureRepository = _pictureRepository ?? new RepositoryBase<Picture>(_context);
            }
        }

        public IRepositoryBase<VehiculeTypeRange> VehiculeTypeRangeRepository
        {
            get
            {
                return _vehiculeTypeRangeRepository = _vehiculeTypeRangeRepository ?? new RepositoryBase<VehiculeTypeRange>(_context);
            }
        }

        public IRepositoryBase<Address> AddressRepository
        {
            get
            {
                return _addressRepository = _addressRepository ?? new RepositoryBase<Address>(_context);
            }
        }

        public IRepositoryBase<Customer> CustomerRepository
        {
            get
            {
                return _customerRepository = _customerRepository ?? new RepositoryBase<Customer>(_context);
            }
        }

        public IRepositoryBase<CodeVerification> CodeVerificationRepository
        {
            get
            {
                return _codeVerificationRepository = _codeVerificationRepository ?? new RepositoryBase<CodeVerification>(_context);
            }
        }

    }
}
