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
        public string DirverLicenceNumber { get; set; }
        [Key]
        public Guid CustomerID { get; set; }

        [NotMapped]
        public Vehicule CurrentVehicule { get; set; }

        [NotMapped]
        public Customer Customer { get; set; }


        public static Driver GetDriverInformation(Guid driverID, IUnitOfWork uow)
        {
            var result = uow.DriverRepository.Get(driverID);
            if(result!=null)
            {
                result.CurrentVehicule = Vehicule.GetCurrentDriverVehiculeDetails(driverID, uow);
            }

            return result;
        }

        public  DriverDto ToDto( )
        {
            DriverDto result = new DriverDto();
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
