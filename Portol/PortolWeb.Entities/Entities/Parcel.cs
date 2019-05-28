using Portol.Common.DTO;
using Portol.Common.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortolWeb.Entities
{
    [Table("tblParcel")]
    public class Parcel
    {
        [Key]
        public Guid ParcelID { get; set; }
        public Guid ParentID { get; set; }
        public ParentType ParentType { get; set; }
        //Centimetre
        public int Width { get; set; }
        //Centimetre
        public int Height { get; set; }
        //Kg
        public int Weight { get; set; }
        //Centimetre
        public int Length { get; set; }

        public decimal Worth { get; set; }

        public static Parcel Create(ParcelDto parcelDto, ParentType parentType, IUnitOfWork uow)
        {
            Parcel parcel = new Parcel();

            parcel.Height = parcelDto.Height;
            parcel.Length = parcelDto.Length;
            parcel.Weight = parcelDto.Weight;
            parcel.Width = parcelDto.Width;
            parcel.Worth = parcelDto.Worth;

            parcel.ParentID = parcelDto.ParentID;
            parcel.ParcelID = parcelDto.ParcelID;
            parcel.ParentType = parentType;

            uow.ParcelRepository.Insert(parcel);
            return parcel;
        }

        public ParcelDto ToDto()
        {
            ParcelDto parcelDto = new ParcelDto();
            parcelDto.Height = this.Height;
            parcelDto.Length  = this.Length;
            parcelDto.ParcelID = this.ParcelID;
            parcelDto.ParentID = this.ParentID;        
            parcelDto.Weight = this.Weight;
            parcelDto.Width = this.Width;
            parcelDto.Worth  = this.Worth;
            return parcelDto;
        }
    }
}
