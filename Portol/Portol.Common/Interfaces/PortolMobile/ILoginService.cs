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
        Task<bool> SendVerificationCode(decimal mobilePhoned, Int32 code);
        Task<bool> VerifyCode(decimal mobilePhoned, Int32 code);
        Task<bool> ResetNewPassword(decimal mobilePhoned, string newPassword);
    }
}
