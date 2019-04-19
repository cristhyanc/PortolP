using Microsoft.EntityFrameworkCore;
using PortolMobile.GeneralTest.Properties;
using PortolWeb.DA;
using PortolWeb.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PortolMobile.GeneralTest.Api.Repository
{
    [Collection("Database collection")]
    public class DatabaseManagementTest 
    {
        IUnitOfWork uow;
        public DatabaseManagementTest(DatabaseFixture fixture)
        {
            uow = fixture.UnitOfWorkDB;
        }

        [Fact]
        public void Tables_Created()
        {
            try
            {               

                DatabaseManagement mng = new DatabaseManagement(uow.Context );
                Assert.True( mng.UpgradeDB(TestResources.SqlFilesUrl));

                uow.AddressRepository.Get(x => x.AddressValidated == true);
                uow.CustomerRepository.Get(x => x.Deleted == true);
                uow.CodeVerificationRepository.Get(x => x.CodeNumber == 1);
                Assert.True(true);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
      
    }
}
