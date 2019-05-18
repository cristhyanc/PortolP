using PortolWeb.Entities;
using PortolWeb.Entities.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortolWeb.DA.Repositories
{
    public class VehiculeTypeRepository: RepositoryBase<VehiculeType>, IVehiculeTypeRepository 
    {
        public VehiculeTypeRepository(DataContext context):base(context)
        {            
        }
    }
}
