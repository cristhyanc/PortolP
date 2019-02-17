using System;
using System.Collections.Generic;
using System.Text;

namespace Portol.Common.Interfaces.PortolWeb
{
   public  interface IDatabaseManagement
    {
        bool UpgradeDB(string scriptsPath);
    }
}
