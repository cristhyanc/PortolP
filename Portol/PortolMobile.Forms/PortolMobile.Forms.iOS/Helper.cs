using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace PortolMobile.Forms.iOS
{
  public  class Helper
    {
        public static UIColor GetUIColor(string hex)
        {
            hex = hex.Replace("#", string.Empty);
            int start = 0;
            byte a = (byte)(Convert.ToUInt32("FF", 16));
            if (hex.Length == 8)
            {
                a = (byte)(Convert.ToUInt32(hex.Substring(start, 2), 16));
                start = 2;
            }

            byte r = (byte)(Convert.ToUInt32(hex.Substring(start, 2), 16));
            start += 2;
            byte g = (byte)(Convert.ToUInt32(hex.Substring(start, 2), 16));
            start += 2;
            byte b = (byte)(Convert.ToUInt32(hex.Substring(start, 2), 16));

            UIColor myBrush = UIColor.FromRGBA(r, g, b, a);
            return myBrush;
        }
    }
}