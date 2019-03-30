﻿using System;
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
       

        public RestClient( )
        {
          
        }

        private async Task<HttpResponseMessage> Call(string url, HttpMethod method, object data = null)
        {
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
                    var error = JsonConvert.DeserializeObject<ApiError>(stringSerialized);
                    throw new AppException(error.StatusDescription);
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
                    var error = JsonConvert.DeserializeObject<ApiError>(stringSerialized);
                    throw new AppException(error.StatusDescription);
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
                    var stringSerialized = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var error = JsonConvert.DeserializeObject<ApiError>(stringSerialized);
                    throw new AppException(error.StatusDescription);
                }
            }           
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}