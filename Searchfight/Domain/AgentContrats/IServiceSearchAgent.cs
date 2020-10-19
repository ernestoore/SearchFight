using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.AgentContrats
{
    public interface IServiceSearchAgent
    {
        Task<string> GetGoogleServiceClient(string queryString);

        Task<string> GetBingServiceClient(string queryString);
    }
}
