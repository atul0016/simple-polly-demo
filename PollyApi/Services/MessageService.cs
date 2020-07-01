using Infrastructure.Resilience.Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using PollyApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollyApi.Services
{
    public interface IMessageService
    {
        Task<string> GetHelloMessage();
        Task<string> GetGoodbyeMessage();
    }
    public class MessageService : IMessageService
    {
        private IMessageRepository _messageRepository;
        private AsyncRetryPolicy _retryPolicy;
        private AsyncCircuitBreakerPolicy _circuitBreakerPolicy;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<string> GetHelloMessage()
        {
            return await PollyRetryRegistry.GetPolicyAsync(5, 2)
                            .ExecuteAsync<string>(async () => 
                                await _messageRepository.GetHelloMessage());
        }

        public async Task<string> GetGoodbyeMessage()
        {
            try
            {
                //Console.WriteLine($"Circuit State: {PollyCircuitBreakerRegistry.GetCircuitBreaker(2).Circ}")
                return await PollyCircuitBreakerRegistry.GetCircuitBreaker(2)
                                .ExecuteAsync<string>(async () =>
                                    await _messageRepository.GetGoodbyeMessage());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
