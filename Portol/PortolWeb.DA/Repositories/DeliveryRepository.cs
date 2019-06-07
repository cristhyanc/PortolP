using PortolWeb.Entities;
using PortolWeb.Entities.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortolWeb.DA.Repositories
{
  public  class DeliveryRepository: RepositoryBase<Delivery>, IDeliveryRepository
    {
        public DeliveryRepository(DataContext context) : base(context)
        {
        }
    }
}
