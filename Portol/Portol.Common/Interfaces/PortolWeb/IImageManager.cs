using Portol.Common.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portol.Common.Interfaces.PortolWeb
{
   public  interface IImageManager
    {
        string SaveFile(byte[] imageArray, string parentID, string imageName, ParentType parentType);
    }
}
