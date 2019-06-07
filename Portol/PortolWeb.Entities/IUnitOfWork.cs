using Microsoft.EntityFrameworkCore.Storage;
using PortolWeb.Entities.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortolWeb.Entities
{
    public interface IUnitOfWork
    {
        IDeliveryRepository DeliveryRepository { get; }
        IParcelRepository ParcelRepository { get; }
        void Dispose();        
        IRepositoryBase<Vehicule> VehiculeRepository { get; }
        IRepositoryBase<PaymentMethod> PaymentMethodRepository { get; }
        IDriverRepository DriverRepository { get; }
        IRepositoryBase<PortolWeb.Entities.VehiculeTypeRange> VehiculeTypeRangeRepository { get; }
        IRepositoryBase<Picture> PictureRepository { get; }
        IVehiculeTypeRepository VehiculeTypeRepository { get; }
        IRepositoryBase<Customer> CustomerRepository { get; }
        IRepositoryBase<Address> AddressRepository { get; }
        IRepositoryBase<CodeVerification> CodeVerificationRepository { get; }
        IDataContext Context { get; }
        IDbContextTransaction BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        void SaveChanges();
    }


}
