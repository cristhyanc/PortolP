using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Base;
using MvvmCross.Logging;
using Newtonsoft.Json;
using Portol.Common;
using Portol.Common.Helper;

namespace PortolMobile.Services.Rest
{
  public  class RestClient : IRestClient
    {
       // private readonly IMvxJsonConverter _jsonConverter;
        private readonly IMvxLog _mvxLog;

        public RestClient( IMvxLog mvxLog)
        {
        //    _jsonConverter = jsonConverter;
            _mvxLog = mvxLog;
        }

        public async Task<TResult> MakeApiCall<TResult>(string url, HttpMethod method, object data = null) where TResult : class
        {
            try
            {
                //  url = url.Replace("http://", "https://");
                
                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage { RequestUri = new Uri(url), Method = method })
                    {
                        // add content
                        if (method != HttpMethod.Get)
                        {
                            var json = JsonConvert.SerializeObject(data);
                            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                        }

                        HttpResponseMessage response = new HttpResponseMessage();
                        try
                        {
                            response = await httpClient.SendAsync(request).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            _mvxLog.ErrorException("MakeApiCall failed", ex);
                            throw new AppException(StringResources.NetworkConnectionError);
                        }

                        var stringSerialized = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        if (response.StatusCode==System.Net.HttpStatusCode.OK)
                        {                            
                            return JsonConvert.DeserializeObject<TResult>(stringSerialized);
                        }
                        else
                        {
                            var error = JsonConvert.DeserializeObject<ApiError>(stringSerialized);
                            throw new AppException(error.StatusDescription);
                        }                       
                    }
                }
            }
            catch (AppException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }

        public async Task<bool> MakeApiCall(string url, HttpMethod method, object data = null) 
        {
            try
            {
                //  url = url.Replace("http://", "https://");

                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage { RequestUri = new Uri(url), Method = method })
                    {
                       
                        if (method != HttpMethod.Get)
                        {
                            var json = JsonConvert.SerializeObject(data);
                            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                        }

                        HttpResponseMessage response = new HttpResponseMessage();
                        try
                        {
                            response = await httpClient.SendAsync(request).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            _mvxLog.ErrorException("MakeApiCall failed", ex);
                        }

                      
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            return true;
                        }
                        else
                        {
                            var stringSerialized = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            var error = JsonConvert.DeserializeObject<ApiError>(stringSerialized);
                            throw new AppException(error.StatusDescription);
                        }
                    }
                }
            }
            catch (AppException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}