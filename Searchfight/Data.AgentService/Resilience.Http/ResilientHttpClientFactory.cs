using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Data.AgentService.Resilience.Http
{
    public class ResilientHttpClientFactory : IResilientHttpClientFactory
    {
        private readonly int _retryCount;
        private readonly int _exceptionsAllowedBeforeBreaking;

        public ResilientHttpClientFactory(int exceptionsAllowedBeforeBreaking = 5, int retryCount = 6)
        {
            _exceptionsAllowedBeforeBreaking = exceptionsAllowedBeforeBreaking;
            _retryCount = retryCount;
        }

        public ResilientHttpClient CreateResilientHttpClient()
        => new ResilientHttpClient((origin) => CreatePolicies());

        private Policy[] CreatePolicies()
        {
            return new Policy[]
                       {
                Policy.Handle<HttpRequestException>()
                .WaitAndRetry(
                    // number of retries
                    _retryCount,
                    // exponential backofff
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    // on retry
                    (exception, timeSpan, retryCount, context) =>
                    {
                        var msg = $"Retry {retryCount} implemented with Polly's RetryPolicy " +
                            $"of {context.PolicyKey} " +
                            $"at {context.OperationKey}, " +
                            $"due to: {exception}.";
                    }),
                Policy.Handle<HttpRequestException>()
                .CircuitBreaker( 
                   // number of exceptions before breaking circuit
                   _exceptionsAllowedBeforeBreaking,
                   // time circuit opened before retry
                   TimeSpan.FromMinutes(1),
                   (exception, duration) =>
                   {
                        // on circuit opened
                   },
                   () =>
                   {
                        // on circuit closed
                   })
                       };
        }
    }
}
