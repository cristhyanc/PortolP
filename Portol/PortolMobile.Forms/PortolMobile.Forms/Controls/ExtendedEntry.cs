using PortolMobile.Forms.Effects;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PortolMobile.Forms.Controls
{
  public  class ExtendedEntry: Entry
    {
        public ExtendedEntry()
        {
            this.Effects.Add(new BorderlessEffect());
        }
    }
}
