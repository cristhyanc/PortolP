using Acr.UserDialogs;
using Microsoft.AppCenter.Crashes;
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
                Crashes.TrackError(ex);

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
    
}
