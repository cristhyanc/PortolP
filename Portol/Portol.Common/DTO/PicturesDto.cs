using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace Portol.Common.DTO
{
   public class PicturesDto
    {
        public Guid PictureID { get; set; }
        public string ImageUrl { get; set; }
        public ImageSource Image { get; set; }

        public PicturesDto()
        {
            PictureID = Guid.NewGuid();
        }
    }
}
