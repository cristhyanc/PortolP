using PortolWeb.Entities;
using PortolWeb.Entities.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortolWeb.DA.Repositories
{
   public class ParcelRepository : RepositoryBase<Parcel>,  IParcelRepository
    {
        public ParcelRepository(DataContext context) : base(context)
        {
        }
    }
}
