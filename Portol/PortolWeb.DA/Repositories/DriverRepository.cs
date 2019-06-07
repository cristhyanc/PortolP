using PortolWeb.Entities;
using PortolWeb.Entities.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortolWeb.DA.Repositories
{
    public class DriverRepository : RepositoryBase<Driver>, IDriverRepository
    {
        public DriverRepository(DataContext context) : base(context)
        {
        }
    }
}
