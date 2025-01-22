using ContactManagement.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.Common.Interfaces
{
    public interface IRabbitMQService : IDisposable
    {
        Task PublishAsync<T>(string routingKey, T message) where T : class;
        Task PublishEmailMessageAsync<T>(T message) where T : EmailMessage;
    }
}
