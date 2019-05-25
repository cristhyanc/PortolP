using Microsoft.EntityFrameworkCore.Storage;
using PortolWeb.DA.Repositories;
using PortolWeb.Entities;
using PortolWeb.Entities.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortolWeb.DA
{
   public  class UnitOfWork: IUnitOfWork, IDisposable
    {
        private readonly DataContext _context;
        private IRepositoryBase<Customer> _customerRepository;
        private IRepositoryBase<CodeVerification> _codeVerificationRepository;
        private IRepositoryBase<Address> _addressRepository;
        private IRepositoryBase<VehiculeTypeRange> _vehiculeTypeRangeRepository;
        private IVehiculeTypeRepository _vehiculeTypeRepository;
        private IRepositoryBase<Picture> _pictureRepository;
        
       

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

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IDataContext Context
        {
            get
            {
                return _context;
            }
        }
            

        private IRepositoryBase<Vehicule> _vehiculeRepository;
        public IRepositoryBase<Vehicule> VehiculeRepository
        {
            get
            {
                return _vehiculeRepository = _vehiculeRepository ?? new RepositoryBase<Vehicule>(_context);
            }
        }


        private IRepositoryBase<PaymentMethod> _paymentMethodRepository;
        public IRepositoryBase<PaymentMethod> PaymentMethodRepository
        {
            get
            {
                return _paymentMethodRepository = _paymentMethodRepository ?? new RepositoryBase<PaymentMethod>(_context);
            }
        }

        private IParcelRepository _parcelRepository;
        public IParcelRepository ParcelRepository
        {
            get
            {
                return _parcelRepository = _parcelRepository ?? new ParcelRepository(_context);
            }
        }

        

        private IDriverRepository _driverRepository;

        public IDriverRepository DriverRepository
        {
            get
            {
                return _driverRepository = _driverRepository ?? new DriverRepository(_context);
            }
        }


        private IDeliveryRepository _deliveryRepository;
       
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
