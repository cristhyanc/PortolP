using PortolWeb.Entities;
using PortolWeb.Entities.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortolWeb.DA.Repositories
{
  public  class DropoffRepository: RepositoryBase<Dropoff>, IDropoffRepository
    {
        public DropoffRepository(DataContext context) : base(context)
        {
        }
    }
}
