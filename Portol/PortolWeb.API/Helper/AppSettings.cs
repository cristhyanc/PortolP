﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortolWeb.API.Helper
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string ConnectionString { get; set; }
        public string DBScriptPath { get; set; }
        public string LogPaht { get; set; }

    }
}
