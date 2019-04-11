using Portol.Common.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Portol.Common.Interfaces.PortolMobile
{
    public interface ILoginService
    {
        Task<UserDto> Authenticate(string email, string password);
        Task<bool> SendVerificationCode(long mobilePhoned, Int32 code);
        Task<bool> VerifyCode(long mobilePhoned, Int32 countryCode, Int32 code);
        Task<bool> ResetNewPassword(long mobilePhoned, string newPassword);
        Task<Boolean> VerifyMobileUniqueness(long mobilePhoned, Int32 code);
        Task<Boolean> VerifyEmailUniqueness(string email);
    }
}
