using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortolMobile.GeneralTest.MockupServices
{
    public class CameraServiceMK : IMedia
    {
        public bool IsCameraAvailable { get; set; }

        public bool IsTakePhotoSupported { get; set; }

        public bool IsPickPhotoSupported { get; set; }

        public bool IsTakeVideoSupported { get; set; }

        public bool IsPickVideoSupported { get; set; }

        public Task<bool> Initialize()
        {
            return Task.Run(()=>  true);
        }

        public Task<MediaFile> PickPhotoAsync(PickMediaOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<MediaFile> PickVideoAsync()
        {
            throw new NotImplementedException();
        }

        public Task<MediaFile> TakePhotoAsync(StoreCameraMediaOptions options)
        {
            MediaFile mediaFile = new MediaFile("TEST",null);
            return Task.Run(() => mediaFile);
        }

        public Task<MediaFile> TakeVideoAsync(StoreVideoOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
