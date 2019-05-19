using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace Portol.Common.DTO
{
   public class PictureDto
    {
        public Guid PictureID { get; set; }
        public string ImageUrl { get; set; }        
        public Guid ParentID { get; set; }
        public Byte[] ImageArray { get; set; }

        public string ImageName { get { return PictureID.ToString() + ".jpg"; } }

        public PictureDto()
        {
            PictureID = Guid.NewGuid();
        }
    }
}
