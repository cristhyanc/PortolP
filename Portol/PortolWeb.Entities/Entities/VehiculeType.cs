using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;
using Portol.Common.DTO;

namespace PortolWeb.Entities
{
    [Table("tblVehiculeType")]
    public class VehiculeType
    {
        [Key]
        public Guid VehiculeTypeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal StartingFee { get; set; }
        public decimal CostPerkilometre { get; set; }
        public long MaximumDistance { get; set; }
        public int MaximumWeight { get; set; }
        public int MaximumWidth { get; set; }
        public int MaximumHeight { get; set; }
        public int MaximumLength { get; set; }

        [NotMapped]
        public long MaximumVolumne
        {
            get
            {
                return MaximumHeight * MaximumLength * MaximumWidth;
            }
        }

        [NotMapped]
        public IEnumerable<VehiculeTypeRange> Ranges { get; set; }


        public static VehiculeType Get(Guid vehiculeTypeID, IUnitOfWork uow)
        {
            var result = uow.VehiculeTypeRepository.Get(vehiculeTypeID);
           if(result!=null)
            {
                result.Ranges = uow.VehiculeTypeRangeRepository.GetAll(x => x.VehiculeTypeID == result.VehiculeTypeID);
            }

            return result;
        }

        public static IEnumerable<VehiculeType> GetAll(IUnitOfWork uow )
        {
           var allTypes = uow.VehiculeTypeRepository.GetAll();
            foreach (VehiculeType item in allTypes)
            {
                item.Ranges = uow.VehiculeTypeRangeRepository.GetAll(x => x.VehiculeTypeID == item.VehiculeTypeID);
            }

            return allTypes;
        }

        public  VehiculeTypeDto ToDto()
        {
            VehiculeTypeDto result = new VehiculeTypeDto();
            result.CostPerkilometre = this.CostPerkilometre;
            result.Description = this.Description;
            result.MaximumDistance = this.MaximumDistance;
            result.MaximumHeight = this.MaximumHeight;
            result.MaximumLength  = this.MaximumLength;           
            result.MaximumWeight  = this.MaximumWeight;
            result.MaximumWidth  = this.MaximumWidth;
            result.Name  = this.Name;
            result.Ranges = new List<VehiculeTypeRangeDto>();
            if (this.Ranges?.Count()>0)
            {
                result.Ranges = this.Ranges.Select(x => x.ToDto()).ToList();
            }
            
            result.StartingFee = this.StartingFee;
            result.VehiculeTypeID = this.VehiculeTypeID;
            return result;

        }
    }
}
