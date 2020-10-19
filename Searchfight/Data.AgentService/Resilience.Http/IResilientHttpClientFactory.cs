using System;
using System.Collections.Generic;
using System.Text;

namespace Data.AgentService.Resilience.Http
{
    public interface IResilientHttpClientFactory
    {
        ResilientHttpClient CreateResilientHttpClient();
    }
}
