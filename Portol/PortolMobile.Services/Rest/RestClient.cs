using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Portol.Common;
using Portol.Common.Helper;

namespace PortolMobile.Services.Rest
{
    public class RestClient : IRestClient
    {

        string Toke;
        public RestClient(string toke)
        {
            this.Toke = toke;
        }

        private async Task<HttpResponseMessage> Call(string url, HttpMethod method, object data = null)
        {
            using (var httpClient = new HttpClient())
            {

                if (method == HttpMethod.Get)
                {
                    if (data != null)
                    {
                        var query = data.ToString();
                        url += "?" + query;
                    }
                }               

                using (var request = new HttpRequestMessage { RequestUri = new Uri(url), Method = method })
                {

                    if(!string.IsNullOrEmpty(Toke))
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", this.Toke);                        
                    }
                   
                    if (method != HttpMethod.Get)
                    {
                     var   jsonContent = JsonConvert.SerializeObject(data);
                        request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    }
                  

                   
                    HttpResponseMessage response = new HttpResponseMessage();
                    try
                    {
                        response = await httpClient.SendAsync(request).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {                      
                        throw new AppException(StringResources.NetworkConnectionError);
                    }

                    return response;
                }
            }
        }

        public async Task<TResult> MakeApiCall<TResult>(string url, HttpMethod method, object data = null) where TResult : class
        {
            try
            {
                HttpResponseMessage response = await Call(url, method, data);
                var stringSerialized = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<TResult>(stringSerialized);
                }
                else
                {
                    if(response.StatusCode == System.Net.HttpStatusCode.NoContent )
                    {
                        return null;
                    }

                    if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized )
                    {
                        throw new AppException(StringResources.LoginAgain);
                    }
                    else
                    {
                        var error = JsonConvert.DeserializeObject<ApiError>(stringSerialized);
                        if (string.IsNullOrEmpty(error.StatusDescription))
                        {
                            error.StatusDescription = error.message;
                        }
                        throw new AppException(error.StatusDescription);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TResult> MakeApiCallRaw<TResult>(string url, HttpMethod method, object data = null) where TResult : struct
        {
            try
            {
                HttpResponseMessage response = await Call(url, method, data);
                var stringSerialized = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<TResult>(stringSerialized);
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return new TResult();
                    }

                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        throw new AppException(StringResources.LoginAgain);
                    }
                    else
                    {
                        var error = JsonConvert.DeserializeObject<ApiError>(stringSerialized);
                        throw new AppException(error.StatusDescription);
                    }
                }
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
                HttpResponseMessage response = await Call(url, method, data);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return false;
                    }

                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        throw new AppException(StringResources.LoginAgain);
                    }
                    else
                    {
                        var stringSerialized = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        var error = JsonConvert.DeserializeObject<ApiError>(stringSerialized);
                        throw new AppException(error.StatusDescription);
                    }
                }
            }           
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}