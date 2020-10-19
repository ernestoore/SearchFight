using Data.AgentService.Agents.Options;
using Data.AgentService.Resilience.Http;
using Domain.AgentContrats;
using Domain.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Data.AgentService.Agents
{
    public class ServiceSearchAgent : IServiceSearchAgent
    {
        private readonly IHttpClient _httpClient;
        private readonly IOptions<WsOptions> _options;

        public ServiceSearchAgent(IHttpClient httpClient, IOptions<WsOptions> options)
        {
            _httpClient = httpClient;
            _options = options;
        }

        public async Task<string> GetBingServiceClient(string queryString)
        {
            var headers = new Dictionary<string, string>();
            headers.Add("Accept", "application/json");
            headers.Add(_options.Value.BingKey, _options.Value.BingToken);


            string response;
            try
            {

                response = await this._httpClient.GetStringAsync($"{_options.Value.UrlBingApi}q={queryString}&customconfig={_options.Value.BingCustomConfig}&mkt=en-US", headers: headers);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }

            if (string.IsNullOrEmpty(response))
            {
                return null;
            }
            else
            {

                var responseBingSearch = JsonConvert.DeserializeObject<ResponseBingSearch>(response);

                return responseBingSearch.webPages.totalEstimatedMatches.ToString();

            }
        }

        public async Task<string> GetGoogleServiceClient(string queryString)
        {
            var headers = new Dictionary<string, string>();
            headers.Add("Accept", "application/json");

            string response;
            try
            {
                
                response = await this._httpClient.GetStringAsync($"{_options.Value.UrlGoogleApi}key={_options.Value.GoogleKey}&cx={_options.Value.GoogleCx}&q={queryString}&alt=json&fields=queries(request(totalResults))", headers: headers);
            }
            catch (Exception ex)
            {
                Console.WriteLine("eRROR: " + ex.Message);
                return "0";
            }

            if (string.IsNullOrEmpty(response))
            {
                return "0";
            }
            else
            {

                var responseGoogleSearch = JsonConvert.DeserializeObject<ResponseGoogleSearch>(response);

                return responseGoogleSearch.queries.request[0].totalResults;

            }
        }

        

        


}
}
