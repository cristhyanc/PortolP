using Acr.UserDialogs;
using Portol.Common;
using Portol.Common.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortolMobile.Forms.Helper
{
    public class ExceptionHelper
    {
        public static void ProcessException(Exception ex, IUserDialogs userDialogs, string pageTitle, string methodName = "")
        {

            if (ex is AppException appException && userDialogs != null)
            {
                userDialogs.Alert(new AlertConfig
                {
                    Message = ex.Message,
                    Title = "",
                    OkText = StringResources.Ok
                });
            }
            else
            {
                if (string.IsNullOrEmpty(methodName))
                {
                    methodName = StringResources.GeneralError;
                }
                //TODO
              //  Logs.Instance.ErrorException(methodName, ex);

                if (userDialogs != null)
                {
                    userDialogs.Alert(new AlertConfig
                    {
                        Message = StringResources.GeneralError,
                        Title = pageTitle,
                        OkText = StringResources.Ok
                    });
                }

            }
        }
    }

    public static class Logs
    {
        //TODO
     //   public static IMvxLog Instance { get; } = Mvx.IoCProvider.Resolve<IMvxLogProvider>().GetLogFor("PortolMobile");

    }
}
