﻿using Portol.Common.Interfaces.PortolWeb;
using PortolWeb.Core.DropoffServices;
using PortolWeb.Core.UserServices;
using PortolWeb.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PortolMobile.GeneralTest.Api.Dropoff
{
    [Collection("Database collection")]
    public class DropoffTest
    {
        IUnitOfWork uow;
        public DropoffTest(DatabaseFixture fixture)
        {
            uow = fixture.UnitOfWorkDB;
        }

      
    }
}
