using Portol.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Portol.Common.Interfaces.PortolMobile
{
   public interface IUserMobileService
    {
        Task<Boolean> CreateNewuser(UserDto newUser);
    }
}
