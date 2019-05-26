using Portol.Common.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortolWeb.Entities
{
    [Table("tblVehicule")]
    public  class Vehicule
    {
        [Key]
        public Guid VehiculeID { get; set; }
        public Guid VehiculeTypeID { get; set; }
        public Guid DriverID { get; set; }
        public bool IsInUsed { get; set; }
        public string Plate { get; set; }

        [NotMapped]
        public VehiculeType VehiculeType { get; set; }

        public static Vehicule GetCurrentDriverVehiculeDetails(Guid driverID, IUnitOfWork uow)
        {
            var result = uow.VehiculeRepository.Get(x => x.DriverID == driverID && x.IsInUsed );
            if(result!=null)
            {
                result.VehiculeType = VehiculeType.Get(result.VehiculeTypeID, uow);
            }
            return result;
        }

        public VehiculeDto ToDto()
        {
            VehiculeDto result = new VehiculeDto();
            result.Plate = this.Plate;
            result.VehiculeID = this.VehiculeID;
            if(VehiculeType!=null)
            {
                result.VehiculeType = this.VehiculeType.ToDto();
            }            
            return result;

        }
    }
}
