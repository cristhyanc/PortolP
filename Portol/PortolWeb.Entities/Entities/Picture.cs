using Portol.Common.DTO;
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
        [Key]
        public Guid PictureID { get; set; }
        public Guid ParentID { get; set; }
        public string ImageUrl { get; set; }
      

        public static Picture Create(PictureDto pictureDto , IUnitOfWork uow)
        {
            Picture picture = new Picture();
            picture.ImageUrl = pictureDto.ImageUrl;
            picture.ParentID = pictureDto.ParentID;
            picture.PictureID = pictureDto.PictureID;
            uow.PictureRepository.Insert(picture);            
            return picture;


        }
    }
}
