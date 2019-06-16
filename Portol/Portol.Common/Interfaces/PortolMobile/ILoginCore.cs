using Portol.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Portol.Common.Interfaces.PortolMobile
{
   public interface ILoginCore
    {      
        Task<CustomerDto> Authenticate(string email, string password);
        Task<MetadataDto> GetMetadata(string appKey);



    }
}
