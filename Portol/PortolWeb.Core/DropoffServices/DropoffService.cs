using Portol.Common.DTO;
using Portol.Common.Interfaces.PortolWeb;
using PortolWeb.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortolWeb.Core.DropoffServices
{
   public class DropoffService: IDropoffService
    {
        private IUnitOfWork _uow;
        public DropoffService(IUnitOfWork uow)
        {
            _uow = uow;
        }

       
    }
}
