using Portol.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Portol.Common.Interfaces.PortolMobile
{
    public interface ILoginService
    {
        Task<UserDto> Authenticate(string email, string password);
        Task<bool> SendVerificationCode(Int32 mobilePhoned);
        Task<bool> VerifyCode(Int32 mobilePhoned, Int32 code);
    }
}
