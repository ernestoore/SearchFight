using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Data.AgentService.Resilience.Http
{
    public interface IHttpClient
    {

        Task<string> GetStringAsync(string uri, Dictionary<string, string> headers = null);
    }
}
