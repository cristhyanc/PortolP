using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PortolMobile.Services.Rest
{
    public interface IRestClient
    {
        Task<bool> MakeApiCall(string url, HttpMethod method, object data = null);

        Task<TResult> MakeApiCall<TResult>(string url, HttpMethod method, object data = null)
                        where TResult : class;

        Task<TResult> MakeApiCallRaw<TResult>(string url, HttpMethod method, object data = null)
            where TResult : struct;
    }
}
