using Portol.Common;
using Portol.Common.Helper;
using Portol.Common.Interfaces.PortolWeb;
using PortolWeb.Entities;
using Sinch.ServerSdk;
using Sinch.ServerSdk.Messaging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PortolWeb.Core.SmsServices
{
    public class SmsService : ISmsService
    {      
        ISmsApi _smsApi;
        IUnitOfWork _uow;
        public SmsService(ISmsApi smsApi, IUnitOfWork uow)
        {
          
            _smsApi = smsApi;
            _uow = uow;
        }

        public async Task SendNewCode(long mobileNumber, Int32 countryCode)
        {
            if (mobileNumber == 0 || countryCode == 0)
            {
                throw new AppException(StringResources.MobileNumberRequiered);
            }

            var codeNumber = "";
            CodeVerification codeVeri = new CodeVerification();
            codeVeri.CountryCode = countryCode;

            //if (!countryCode.Contains("+"))
            //{
            codeNumber = "+" + countryCode.ToString() + mobileNumber.ToString();
            //}

            //try
            //{
                Random random = new Random();
                int code = random.Next(1000, 9999);
                string fullNumber = codeNumber;
                //TODO: string resource
                var result = await _smsApi.Sms(fullNumber, "Your SMS Code from Portol is: " + code.ToString()).Send();
                await Task.Delay(TimeSpan.FromSeconds(10)); // May take a second or two to be delivered.

                var smsMessageStatusResponse = await _smsApi.GetSmsStatus(result.MessageId);
                codeVeri.CodeNumber = code;
                codeVeri.PhoneNumber = mobileNumber;
                _uow.CodeVerificationRepository.Insert(codeVeri);
                _uow.SaveChanges();
            //}
            //catch (Exception ex)
            //{

            //    throw ex;
            //}
          
        }
    }
}
