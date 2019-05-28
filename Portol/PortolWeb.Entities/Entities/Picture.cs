using Portol.Common.DTO;
using Portol.Common.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortolWeb.Entities
{
    [Table("tblPicture")]
    public class Picture
    {
        public Picture()
        {
            PictureID = Guid.NewGuid();
        }

        [Key]
        public Guid PictureID { get; set; }
        public Guid ParentID { get; set; }
        public string ImageUrl { get; set; }
        public ParentType ParentType { get; set; }

        [NotMapped]
        public string ImageName { get { return PictureID.ToString() + ".jpg"; } }

        public static Picture Create(PictureDto pictureDto , ParentType parentType, IUnitOfWork uow)
        {
            Picture picture = new Picture();
            picture.ImageUrl = pictureDto.ImageUrl;
            picture.ParentID = pictureDto.ParentID;
            picture.PictureID = pictureDto.PictureID;
            picture.ParentType = parentType;
            uow.PictureRepository.Insert(picture);            
            return picture;


        }

        public PictureDto ToDto()
        {
            PictureDto pictureDto = new PictureDto();
            pictureDto.PictureID = this.PictureID;
            pictureDto.ParentID = this.ParentID;
            pictureDto.ImageUrl = this.ImageUrl;
            return pictureDto;
        }
    }
}
