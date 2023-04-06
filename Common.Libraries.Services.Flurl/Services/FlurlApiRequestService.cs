using Common.Libraries.Services.Services;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Libraries.Services.Flurl.Services
{
    public class FlurlApiRequestService : IApiRequestService
    {
        public async Task<(T result, int statusCode)> PostAsync<T>(string url, Dictionary<string, string> headers, object data, Func<string, string, int, Task> logRequest = null) where T : class
        {

            var request = url.WithTimeout(TimeSpan.FromSeconds(60));

            if (headers != null && headers.Count > 0)
            {
                foreach (var header in headers)
                {
                    request = request.WithHeader(header.Key, header.Value);
                }
            }
            var response = await request.PostJsonAsync(data);
            var r = await response.GetStringAsync();
            if (logRequest != null)
            {
                var req = response.ResponseMessage?.RequestMessage?.ToString();
                var res = r;
                await logRequest(req, res, response.StatusCode);
            }
            var result = await response.GetJsonAsync<T>();
            return (result, response.StatusCode);
        }
        public async Task<(T result, int statusCode)> DeleteAsync<T>(string url, Dictionary<string, string> headers, object data, Func<string, string, int, Task> logRequest = null) where T : class
        {

            var request = url.WithTimeout(TimeSpan.FromSeconds(60));

            if (headers != null && headers.Count > 0)
            {
                foreach (var header in headers)
                {
                    request = request.WithHeader(header.Key, header.Value);
                }
            }
            var response = await url.SendJsonAsync(HttpMethod.Delete, data);
            if (logRequest != null)
            {
                var req = response.ResponseMessage.RequestMessage.ToString();
                var res = response.ResponseMessage.ToString();
                await logRequest(req, res, response.StatusCode);
            }
            var result = await response.GetJsonAsync<T>();
            return (result, response.StatusCode);
        }
        public async Task<(T result, int statusCode)> PutAsync<T>(string url, Dictionary<string, string> headers, object data, Func<string, string, int, Task> logRequest = null) where T : class
        {

            var request = url.WithTimeout(TimeSpan.FromSeconds(60));

            if (headers != null && headers.Count > 0)
            {
                foreach (var header in headers)
                {
                    request = request.WithHeader(header.Key, header.Value);
                }
            }
            var response = await request.PutJsonAsync(data);
            if (logRequest != null)
            {
                var req = response.ResponseMessage.RequestMessage.ToString();
                var res = response.ResponseMessage.ToString();
                await logRequest(req, res, response.StatusCode);
            }
            var result = await response.GetJsonAsync<T>();
            return (result, response.StatusCode);
        }
        public async Task<(T result, int statusCode)> PatchAsync<T>(string url, Dictionary<string, string> headers, object data, Func<string, string, int, Task> logRequest = null) where T : class
        {

            var request = url.WithTimeout(TimeSpan.FromSeconds(60));

            if (headers != null && headers.Count > 0)
            {
                foreach (var header in headers)
                {
                    request = request.WithHeader(header.Key, header.Value);
                }
            }
            var response = await request.PatchJsonAsync(data);
            if (logRequest != null)
            {
                var req = response.ResponseMessage.RequestMessage.ToString();
                var res = response.ResponseMessage.ToString();
                await logRequest(req, res, response.StatusCode);
            }
            var result = await response.GetJsonAsync<T>();
            return (result, response.StatusCode);
        }
        public async Task<(T result, int statusCode)> GetAsync<T>(string url, Dictionary<string, string> headers, Func<string, string, int, Task> logRequest = null) where T : class
        {
            var request = url.WithTimeout(TimeSpan.FromSeconds(60));

            if (headers != null && headers.Count > 0)
            {
                foreach (var header in headers)
                {
                    request = request.WithHeader(header.Key, header.Value);
                }
            }
            var response = await request.GetAsync();
            if (logRequest != null)
            {
                var req = response.ResponseMessage.RequestMessage.ToString();
                var res = response.ResponseMessage.ToString();
                await logRequest(req, res, response.StatusCode);
            }
            var result = await response.GetJsonAsync<T>();
            return (result, response.StatusCode);
        }
    }
}
