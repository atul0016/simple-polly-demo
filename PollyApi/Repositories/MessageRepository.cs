using Microsoft.Extensions.Options;
using PollyApi.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollyApi.Repositories
{
    public interface IMessageRepository
    {
        Task<string> GetHelloMessage();
        Task<string> GetGoodbyeMessage();
    }
    public class MessageRepository : IMessageRepository
    {
        private MessageOptions _messageOptions;

        public MessageRepository(IOptions<MessageOptions> messageOptions)
        {
            _messageOptions = messageOptions.Value;
        }

        public async Task<string> GetHelloMessage()
        {
            Console.WriteLine("MessageRepostiory GetHelloMessage running..");
            ThrowRandomException();
            return _messageOptions.HelloMessage;
        }

        public async Task<string> GetGoodbyeMessage()
        {
            Console.WriteLine("MessageRepository GetGoodbyeMessage running..");
            ThrowRandomException();
            return _messageOptions.GoodbyeMessage;
        }

        private void ThrowRandomException()
        {
            var random = new Random().Next(0, 10);

            if (random > 5)
            {
                Console.WriteLine("Error! Throwing Exception");
                throw new Exception("Exception in MessageRepository");
            }
        }
    }
}
