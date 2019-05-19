using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PortolMobile.Forms.Services.Images
{
    public class ImageManager
    {

        public const string TEMP_IMAGE_FOLDER = "tempimages";


        public static async Task<byte[]> GetPictureFromDisk(string urlFile)
        {

            var result = await Task.Run(() =>
            {
                if (File.Exists(urlFile))
                {
                    FileStream stream = File.Open(urlFile, FileMode.Open);
                    if (stream != null)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            return ms.ToArray();
                        }
                    }                    
                }
                return null;
            });

            return result;
        }


        public static async Task<string> SavePictureToDisk(Stream newImage)
        {
            var result = await Task.Run(() =>
            {
                if (newImage != null)
                {
                    Byte[] ImageByteArray;
                    string finalPath = "";
                    string folPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/" + TEMP_IMAGE_FOLDER;

                    if (!Directory.Exists(folPath))
                    {
                        Directory.CreateDirectory(folPath);
                    }


                    finalPath = folPath + "/" + Guid.NewGuid().ToString() + ".jpg";
                    FileStream myFile = null;

                    if (!File.Exists(finalPath))
                    {
                        myFile = File.Create(finalPath);
                    }
                    else
                    {
                        myFile = File.OpenWrite(finalPath);
                    }

                    using (var memoryStream = new System.IO.MemoryStream())
                    {
                        newImage.CopyTo(memoryStream);
                        newImage.Dispose();
                        ImageByteArray = memoryStream.ToArray();
                    }


                    SKBitmap bitmap = SKBitmap.Decode(ImageByteArray);
                    var imageSK = SKImage.FromBitmap(bitmap);
                    SKData d = imageSK.Encode(SKEncodedImageFormat.Jpeg, 40);
                    d.SaveTo(myFile);
                    bitmap.Dispose();
                    imageSK.Dispose();

                    myFile.Close();
                    myFile.Dispose();

                    return finalPath;
                }
                return "";
            });
            return result;
        }
    }
}
