using Microsoft.EntityFrameworkCore;
using PortolMobile.GeneralTest.MockupServices;
using PortolMobile.GeneralTest.Properties;
using PortolWeb.DA;
using PortolWeb.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PortolMobile.GeneralTest.Api
{
   public class Helper
    {
       
    }

    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

    public class DatabaseFixture : IDisposable
    {
       
        public DataContext DataContext { get; private set; }
        public UnitOfWork UnitOfWorkDB { get; private set; }

        public DatabaseFixture()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer(TestResources.ConnectionString);
            DataContext = new DataContext(optionsBuilder.Options);

            UnitOfWorkDB = new UnitOfWork(DataContext);
            DatabaseManagement mng = new DatabaseManagement(UnitOfWorkDB.Context);
            mng.UpgradeDB(TestResources.SqlFilesUrl);
            SetupData(UnitOfWorkDB);

            LoginServiceMK loginServiceMK = new LoginServiceMK(UnitOfWorkDB);
            Forms.SessionData.LoginUser(loginServiceMK, "cristhyan@outlook.com", "").Wait();
           
        }

        public void Dispose()
        {
            // ... clean up test data from the database ...
        }

        private  void SetupData(IUnitOfWork uow)
        {
            SetupCustomerInfo(uow);
            SetupCodeVerificationInfo(uow);
        }

        private  void SetupCustomerInfo(IUnitOfWork uow)
        {
            uow.Context.Database.ExecuteSqlCommand("truncate table tblCustomer");
            uow.Context.Database.ExecuteSqlCommand("INSERT INTO tblCustomer([FirstName],[LastName],[Email],[DOB],[PhoneNumber],[PhoneCountryCode]) VALUES('Cristhyan','Cardona','cristhyan@outlook.com','11/17/1984','405593358',61),('Chancellor','Josiah','a.arcu.Sed@magna.ca','10/06/1985','48363404',81),('Lillian','Cyrus','non.justo.Proin@vel.net','01/16/1991','49948698',79),('Burke','Wing','cursus.et@anteipsumprimis.org','06/05/1997','19301071',77),('Cally','Sharon','Vivamus@eratEtiam.ca','04/02/1994','44947427',69),('India','Whilemina','cursus.vestibulum@augueacipsum.edu','12/12/1986','83598764',27),('Zeus','Kathleen','tincidunt.vehicula.risus@bibendumullamcorperDuis.co.uk','07/29/1984','89151846',74),('Blair','Ariana','dolor.dapibus@adipiscingligula.ca','08/01/1986','85353275',12),('Rowan','Elton','dis.parturient@libero.com','12/23/1988','38560428',5),('Reagan','Aline','lacinia@necorci.com','04/01/1992','35588273',19);" +
            "INSERT INTO tblCustomer([FirstName],[LastName],[Email],[DOB],[PhoneNumber],[PhoneCountryCode]) VALUES('Odysseus','Hilda','sed.tortor.Integer@nuncrisusvarius.org','10/27/1995','22489159',75),('Cairo','Jakeem','Cras.convallis@etmagnis.ca','10/10/1983','39072847',20),('Megan','Risa','Nam.ligula@Sedeget.ca','12/02/1984','71924768',4),('Robin','Pandora','Integer@pede.ca','02/17/1994','76708918',35),('Patience','Shay','malesuada.fames.ac@nisiAenean.com','07/03/1981','05354889',47),('Xaviera','Aladdin','risus@ullamcorpervelitin.edu','08/03/1999','88837627',6),('Maris','Samantha','ipsum@Duis.edu','10/31/1987','02342581',74),('Claire','Orson','consectetuer.ipsum.nunc@idenimCurabitur.com','08/08/1999','41137085',76),('Cameron','Zelda','risus@purusgravida.com','10/12/1998','78725448',92),('Wallace','Orla','aliquet.Phasellus@loremauctor.com','12/09/1981','40978846',1);" +
            "INSERT INTO tblCustomer([FirstName],[LastName],[Email],[DOB],[PhoneNumber],[PhoneCountryCode]) VALUES('Courtney','Orli','ipsum.Suspendisse@ametornarelectus.org','09/09/1989','90777400',89),('Marsden','Mona','lobortis.tellus@duiin.co.uk','07/17/1992','67458600',66),('Adele','Cooper','libero@interdumNunc.com','06/11/1991','63165926',16),('Samuel','Diana','Suspendisse.eleifend.Cras@utpharetra.org','06/18/1996','91989653',83),('Tanisha','Emma','magna.a@vitaesodales.ca','05/26/1982','48063111',3),('Mikayla','Nash','egestas.lacinia.Sed@congueturpis.ca','07/18/1983','85257588',62),('Emerson','Quail','nec.tempus.mauris@laciniaatiaculis.co.uk','04/03/1986','51191773',85),('Beatrice','Marah','est@consectetuermaurisid.net','07/24/1991','23457251',32),('Cally','Lois','tempus.non@tellusAenean.net','04/22/1985','77402616',59),('Tatum','Nadine','lobortis@cursus.net','12/11/1997','65411632',61);" +
            "INSERT INTO tblCustomer([FirstName],[LastName],[Email],[DOB],[PhoneNumber],[PhoneCountryCode]) VALUES('Abbot','Shaine','mus.Donec@euarcu.com','08/29/1991','76953609',71),('Venus','Jaime','dui@non.com','01/30/1982','49650498',43),('Autumn','Keane','augue.ac.ipsum@iaculis.org','02/14/1989','91344725',61),('Benedict','Brock','gravida.mauris@elementumlorem.edu','06/04/1993','01198164',23),('Jaime','Keaton','diam.Pellentesque.habitant@magnisdis.com','11/27/1982','41966536',90),('Rashad','Fulton','ipsum@rutrumFusce.co.uk','05/23/1996','12967478',10),('Cathleen','Lynn','sed@felisDonectempor.com','08/26/1992','13965116',88),('Jaquelyn','Chase','sollicitudin@etnunc.com','07/28/1993','04465960',51),('William','Irma','lorem.eget@elit.edu','03/29/1982','25014561',32),('Owen','Palmer','gravida.molestie@Mauris.org','08/28/1984','39500980',45);" +
            "INSERT INTO tblCustomer([FirstName],[LastName],[Email],[DOB],[PhoneNumber],[PhoneCountryCode]) VALUES('Idona','Ulla','et.ultrices@ornare.org','04/11/1988','54405556',56),('Davis','Josephine','Donec.nibh.Quisque@auctorodioa.edu','09/07/1997','88993370',46),('Kyla','Bruce','nec.tempus@Nuncmauris.ca','01/08/1995','96648955',12),('Kimberley','Mara','sed.turpis.nec@eu.net','05/10/1984','10675693',61),('Jack','Ursa','lobortis.ultrices@nectempus.edu','03/07/1998','65344058',9),('Alyssa','Madaline','interdum@mauris.co.uk','01/08/1988','94347463',80),('James','Angela','montes@lacusvariuset.co.uk','08/16/1993','39022378',10),('Yvonne','Joel','velit.Quisque.varius@urnaUt.org','08/11/1999','96009533',50),('Sarah','Dean','Praesent.luctus.Curabitur@porttitortellus.org','05/19/1997','44127496',68),('Liberty','Griffin','vel@velit.org','10/28/1988','24429165',45);" +
            "INSERT INTO tblCustomer([FirstName],[LastName],[Email],[DOB],[PhoneNumber],[PhoneCountryCode]) VALUES('Grady','Audra','lectus@dapibusligulaAliquam.edu','01/09/2000','85481352',56),('Ebony','Mechelle','Etiam@molestietellusAenean.org','07/30/1987','83497941',32),('Barbara','Matthew','massa.non.ante@interdumfeugiat.com','12/30/1989','81366087',92),('Dolan','Reece','enim.consequat.purus@etultrices.ca','08/30/1991','61555610',39),('Dominique','Kay','mauris@tinciduntcongue.net','10/14/1998','82594935',92),('Graham','Dane','interdum.libero.dui@Curabiturut.co.uk','11/27/1999','14187830',41),('Phoebe','Heidi','tincidunt.tempus@Phasellus.ca','02/09/1994','13849370',13),('Mallory','Uriah','porttitor.eros.nec@dictumeuplacerat.co.uk','02/24/2000','33532856',88),('Jennifer','Ramona','ultricies@sitamet.edu','10/29/1983','70967795',91),('Moana','Keiko','Donec.feugiat@auctor.ca','02/03/1992','87632391',24);" +
            "INSERT INTO tblCustomer([FirstName],[LastName],[Email],[DOB],[PhoneNumber],[PhoneCountryCode]) VALUES('Anne','Eagan','urna.Vivamus.molestie@Aeneaneget.org','02/28/1991','22998887',24),('Chase','Rosalyn','dis@semper.co.uk','11/19/1991','50491667',85),('Quinn','Cassandra','justo@viverra.net','09/14/1986','29243945',67),('Mariko','Isaiah','egestas@ametdapibus.net','06/29/1984','52043586',50),('Mariam','Ashton','dui.Suspendisse@non.net','08/26/1983','98906614',79),('Sheila','Henry','cursus.diam@magna.ca','08/16/1988','07447549',21),('Nehru','Stephen','magna.Sed.eu@elit.net','03/27/1985','77143069',90),('Josiah','Doris','sem.Pellentesque.ut@Sed.net','06/03/1989','35269686',39),('Rogan','Oren','nec@vulputatenisisem.edu','08/28/1982','91579496',61),('Aidan','Zorita','justo.nec@Nullam.org','08/20/1990','05605409',13);" +
            "INSERT INTO tblCustomer([FirstName],[LastName],[Email],[DOB],[PhoneNumber],[PhoneCountryCode]) VALUES('Galvin','Scarlet','montes@Nullam.co.uk','04/08/1983','10960577',15),('Ross','Colt','pede.Praesent.eu@dui.org','01/19/1991','32342341',78),('Nelle','Demetrius','nulla.vulputate.dui@Aenean.com','12/18/1992','81848318',65),('Cassidy','Elliott','tempor.diam@sollicitudin.com','02/19/1988','93308728',28),('Rhonda','Emi','aliquam.adipiscing.lacus@sollicitudinorcisem.org','01/19/1986','89023055',8),('Veronica','Lucy','nec.diam@adipiscingelitCurabitur.edu','05/09/1988','78315916',13),('Aristotle','Janna','tellus@Nullamnisl.com','09/15/1990','01877167',18),('Camden','Jacob','ipsum@egestasrhoncusProin.com','08/16/1985','67474193',53),('Cameran','Leo','magna.et@vulputatemaurissagittis.org','07/21/1991','43479862',45),('Remedios','Castor','Morbi.vehicula@augue.com','03/18/1983','58872150',17);" +
            "INSERT INTO tblCustomer([FirstName],[LastName],[Email],[DOB],[PhoneNumber],[PhoneCountryCode]) VALUES('Kelsie','Walker','convallis.ante@Nulla.net','09/17/1985','92952938',82),('Phyllis','Nathaniel','Aliquam@dapibusquam.com','07/14/1989','52894079',22),('Genevieve','Brady','et.arcu@aliquet.co.uk','03/08/1999','86104124',92),('Madaline','Desiree','Sed.eu@Integersemelit.edu','02/28/1997','82918197',56),('Conan','Forrest','mollis@fringilla.com','01/24/1982','25951366',31),('Mason','Kaye','tincidunt.dui.augue@Sedeu.co.uk','01/26/1989','52154193',31),('Francesca','Rebekah','Nunc.mauris.elit@luctusaliquetodio.ca','12/03/1981','48794959',84),('Grace','Cailin','eros@mattis.net','05/08/1991','64732149',23),('Jasmine','Erasmus','lobortis.quis@feugiatLorem.ca','08/24/1982','33840843',93),('Scarlet','Boris','orci.quis.lectus@Nulla.net','02/25/1996','14660169',41);" +
            "INSERT INTO tblCustomer([FirstName],[LastName],[Email],[DOB],[PhoneNumber],[PhoneCountryCode]) VALUES('Odette','Kennan','ipsum.Phasellus.vitae@acmattis.com','10/31/1985','77719043',45),('Uriel','Addison','vehicula.Pellentesque.tincidunt@etarcuimperdiet.org','10/13/1989','15821656',36),('Irma','Maile','cursus.Integer.mollis@pellentesqueSed.co.uk','02/09/1995','44351652',57),('Olga','Craig','Sed.eget.lacus@Aeneanegetmetus.ca','09/03/1989','71329177',7),('Marvin','Jordan','augue@Nunc.ca','01/20/1991','10841714',67),('Wang','Hadassah','mauris@odiovel.edu','05/17/1994','77207513',7),('Stuart','Marsden','fermentum.metus.Aenean@Nullamenim.ca','01/02/1988','31525704',23),('Hyacinth','Abigail','luctus@convallisante.co.uk','11/14/1992','13075280',5),('Coby','Jerome','aliquam@quamdignissim.com','06/08/1984','60164056',72),('Jessamine','Sarah','consectetuer.ipsum@porttitorscelerisque.ca','03/06/1989','37203403',54);");

        }

        private  void SetupCodeVerificationInfo(IUnitOfWork uow)
        {
            uow.Context.Database.ExecuteSqlCommand("truncate table tblCodeVerification");
            uow.Context.Database.ExecuteSqlCommand(
            "insert into tblCodeVerification (CodeNumber, PhoneNumber, CountryCode, Created) values (6057, '9507197870', 14, '4/20/2019');" +
            "insert into tblCodeVerification (CodeNumber, PhoneNumber, CountryCode, Created) values (3804, '5949849170', 83, '4/24/2019');" +
            "insert into tblCodeVerification (CodeNumber, PhoneNumber, CountryCode, Created) values (7426, '6337566165', 19, '4/24/2019');" +
            "insert into tblCodeVerification (CodeNumber, PhoneNumber, CountryCode, Created) values (2710, '7264971097', 8, '4/25/2019');" +
            "insert into tblCodeVerification (CodeNumber, PhoneNumber, CountryCode, Created) values (3899, '6443024302', 30, '4/22/2019');" +
            "insert into tblCodeVerification (CodeNumber, PhoneNumber, CountryCode, Created) values (2636, '3676299726', 77, '4/19/2019');" +
            "insert into tblCodeVerification (CodeNumber, PhoneNumber, CountryCode, Created) values (7155, '3114119227', 79, '4/19/2019');" +
            "insert into tblCodeVerification (CodeNumber, PhoneNumber, CountryCode, Created) values (9876, '2742042813', 25, '4/19/2019');" +
            "insert into tblCodeVerification (CodeNumber, PhoneNumber, CountryCode, Created) values (6704, '5324595220', 1, '4/19/2019');" +
            "insert into tblCodeVerification (CodeNumber, PhoneNumber, CountryCode, Created) values (8413, '5534455863', 41, '4/21/2019');"
            );

        }


        private void CleanDataBase(IUnitOfWork uow)
        {
            uow.Context.Database.ExecuteSqlCommand("truncate table tblAddress");
            uow.Context.Database.ExecuteSqlCommand("truncate table tblBusiness");
            uow.Context.Database.ExecuteSqlCommand("truncate table tblCodeVerification");
            uow.Context.Database.ExecuteSqlCommand("truncate table tblCustomer");
            uow.Context.Database.ExecuteSqlCommand("truncate table tblDelivery");
            uow.Context.Database.ExecuteSqlCommand("truncate table tblDriver");
            uow.Context.Database.ExecuteSqlCommand("truncate table tblGallery");
            uow.Context.Database.ExecuteSqlCommand("truncate table tblGalleryItem");
            uow.Context.Database.ExecuteSqlCommand("truncate table tblParcel");
            uow.Context.Database.ExecuteSqlCommand("truncate table tblParcelItem");
        }
    }

}
