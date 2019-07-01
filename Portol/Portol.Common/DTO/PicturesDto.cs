using Portol.Common.Helper;
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


        public string _imageUrl;
        public string ImageUrl {
            get
            {
                if (string.IsNullOrEmpty(_imageUrl))
                {
                    _imageUrl = Constants.BaseUrl + "/images/profile_avatar.png";
                }
                return _imageUrl;
            }
            set
            {
                _imageUrl = value;
            }

        } 
        public Guid ParentID { get; set; }
        public Byte[] ImageArray { get; set; }

        public string ImageName { get { return PictureID.ToString() + ".jpg"; } }

        public PictureDto()
        {
            PictureID = Guid.NewGuid();
        }
    }
}
