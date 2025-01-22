using ContactManagement.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.Common.Behaviors
{
    public class DomainEventPublishingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
      where TRequest : notnull
    {
        private readonly IPublisher _publisher;

        public DomainEventPublishingBehavior(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = await next();

            if (response is IHasDomainEvents hasEvents)
            {
                var events = hasEvents.DomainEvents.ToArray();
                hasEvents.ClearDomainEvents();

                foreach (var domainEvent in events)
                {
                    await _publisher.Publish(domainEvent, cancellationToken);
                }
            }

            return response;
        }
    }
}
