using Portol.Common.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortolWeb.Entities
{
    [Table("tblDriver")]
    public  class Driver
    {
        
        public bool IsOnDuty { get; set; }

        public string DirverLicenceNumber { get; set; }
        public decimal Rating { get; set; }

        

        [Key]
        public Guid CustomerID { get; set; }

        [NotMapped]
        public Vehicule CurrentVehicule { get; set; }

        [NotMapped]
        public Customer Customer { get; set; }

        public static IEnumerable<Driver> GetAvailableDrivers(IUnitOfWork uow)
        {
            return uow.DriverRepository.GetAll(x => x.IsOnDuty);
        }

        public static Driver GetDriverInformation(Guid driverID, IUnitOfWork uow)
        {
            var result = uow.DriverRepository.Get(driverID);
            if(result!=null)
            {
                result.Customer =Entities.Customer.GetCustomerDetails(uow,driverID);
                result.CurrentVehicule = Vehicule.GetCurrentDriverVehiculeDetails(driverID, uow);
            }

            return result;
        }

        public  DriverDto ToDto( )
        {
            DriverDto result = new DriverDto();
           
            result.Rating = this.Rating;
            result.DirverLicenceNumber = this.DirverLicenceNumber;
            if(CurrentVehicule!=null)
            {
                result.CurrentVehicule = CurrentVehicule.ToDto();
            }
            
            if(this.Customer!=null)
            {
                result.Details = this.Customer.ToDto();
            }

            return result;
        }
    }
}
