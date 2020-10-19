using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Wrap;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Data.AgentService.Resilience.Http
{
    public class ResilientHttpClient : IHttpClient
    {
        private readonly System.Net.Http.HttpClient _client;
        private readonly Func<string, IEnumerable<Policy>> _policyCreator;
        private ConcurrentDictionary<string, PolicyWrap> _policyWrappers;

        public ResilientHttpClient(Func<string, IEnumerable<Policy>> policyCreator)
        {
            _client = new System.Net.Http.HttpClient();
            _policyCreator = policyCreator;
            _policyWrappers = new ConcurrentDictionary<string, PolicyWrap>();
        }

        
        private async Task<T> HttpInvoker<T>(string origin, Func<Context, Task<T>> action)
        {
            var normalizedOrigin = NormalizeOrigin(origin);

            if (!_policyWrappers.TryGetValue(normalizedOrigin, out PolicyWrap policyWrap))
            {
                var policyData = _policyCreator(normalizedOrigin).ToArray();
                policyWrap = Policy.Wrap(_policyCreator(normalizedOrigin).ToArray());
                _policyWrappers.TryAdd(normalizedOrigin, policyWrap);
            }

            Context contextD = new Context(normalizedOrigin);

            // Executes the action applying all 
            // the policies defined in the wrapper
            return await policyWrap.Execute(action, contextD);
        }

        private static string NormalizeOrigin(string origin)
        {
            return origin?.Trim()?.ToLower();
        }

        private static string GetOriginFromUri(string uri)
        {
            var url = new Uri(uri);

            var origin = $"{url.Scheme}://{url.DnsSafeHost}:{url.Port}";

            return origin;
        }

        public Task<string> GetStringAsync(string uri, Dictionary<string, string> headers = null)
        {
            var origin = GetOriginFromUri(uri);
            return HttpInvoker(origin, async Context =>
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        requestMessage.Headers.TryAddWithoutValidation(item.Key, item.Value);
                    }
                }

                var response = await _client.SendAsync(requestMessage);

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    throw new HttpRequestException();
                }

                return await response.Content.ReadAsStringAsync();

            });

        }
    }
}
