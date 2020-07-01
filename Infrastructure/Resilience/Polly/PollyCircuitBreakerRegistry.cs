using Polly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Resilience.Polly
{
    public static class PollyCircuitBreakerRegistry
    {
        public static AsyncPolicy GetCircuitBreaker(int exceptionsAllowed)
        {
            return Policy
                    .Handle<Exception>()
                    .CircuitBreakerAsync(exceptionsAllowed, TimeSpan.FromSeconds(1),
                    (ex, t) =>
                    {
                        Console.WriteLine("Circuit broken .. !");
                    },
                    () =>
                    {
                        Console.WriteLine("Circuit reset .. !");
                    });
        }
    }
}
