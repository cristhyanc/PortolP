using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portol.Common.Helper
{
    public class ApiError
    {
        public int StatusCode { get;  set; }

        public string StatusDescription { get;  set; }

        //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Message { get;  set; }

        public ApiError(int statusCode, string statusDescription)
        {
            this.StatusCode = statusCode;
            this.StatusDescription = statusDescription;
        }

        public ApiError(int statusCode, string statusDescription, string message)
            : this(statusCode, statusDescription)
        {
            this.Message = message;
        }
        
        public ApiError()
        {

        }
    }
}
