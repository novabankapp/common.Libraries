using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Libraries.Services.Services
{
    public interface IApiRequestService
    {
        Task<(T result, int statusCode)> DeleteAsync<T>(string url, Dictionary<string, string> headers, object data, Func<string, string, int, Task> logRequest = null) where T : class;
        Task<(T result, int statusCode)> GetAsync<T>(string url, Dictionary<string, string> headers, Func<string, string, int, Task> logRequest = null) where T : class;
        Task<(T result, int statusCode)> PatchAsync<T>(string url, Dictionary<string, string> headers, object data, Func<string, string, int, Task> logRequest = null) where T : class;
        Task<(T result, int statusCode)> PostAsync<T>(string url, Dictionary<string, string> headers, object data, Func<string, string, int, Task> logRequest = null) where T : class;
        Task<(T result, int statusCode)> PutAsync<T>(string url, Dictionary<string, string> headers, object data, Func<string, string, int, Task> logRequest = null) where T : class;
    }
    public class Auth
    {
        public string Token { get; set; }
        public AuthType AuthType { get; set; }
    }
    public enum AuthType
    {
        BEARER,
        BASIC

    }
}
