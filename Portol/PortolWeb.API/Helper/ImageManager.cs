using Portol.Common.Helper;
using Portol.Common.Interfaces.PortolWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace PortolWeb.API.Helper
{
    public class ImageManager: IImageManager
    {
        const string DELIVERY_IMAGES_FOLDER = "deliveryImages";
        const string PROFILE_IMAGES_FOLDER = "profileImages";
        const string IMAGES_FOLDER = "Images";
        AppSettings _appSettings;
        IHostingEnvironment _hosting;
        public string ServerUrl
        {
            get
            {
                return _appSettings.ServerUrl;
            }
        }

        public ImageManager(AppSettings appSettings, IHostingEnvironment hosting)
        {
            _appSettings = appSettings;
            _hosting = hosting;
        }

        public string SaveFile(byte[] imageArray, string parentID, string imageName, ParentType parentType)
        {
            InitializeFolders();
            string path = IMAGES_FOLDER;
            FileStream finalFile;
            switch (parentType)
            {
                case ParentType.Customer:
                    path += "/" + PROFILE_IMAGES_FOLDER;
                    break;
                case ParentType.Delivery:
                    path += "/" + DELIVERY_IMAGES_FOLDER;
                    break;
                default:
                    break;
            }

            path += "/" + parentID;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path += "/" + imageName;
            finalFile = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
            finalFile.Write(imageArray, 0, imageArray.Length);
            finalFile.Close();
            finalFile.Dispose();

            return ServerUrl + "/" + path;
        }

        private void InitializeFolders()
        {
            string path = IMAGES_FOLDER;
            if (!Directory.Exists (path))
            {
                Directory.CreateDirectory(path);
            }

            path += "/" + PROFILE_IMAGES_FOLDER;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = IMAGES_FOLDER;
            path += "/" + DELIVERY_IMAGES_FOLDER;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }


    }
}
