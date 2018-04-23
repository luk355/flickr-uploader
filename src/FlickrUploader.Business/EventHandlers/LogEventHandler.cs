using System.Threading;
using System.Threading.Tasks;
using FlickrUploader.Core.Eventing;
using Serilog;

namespace FlickrUploader.Business.EventHandlers
{
    public class LogEventHandler : IDomainEventHandler<IDomainEvent>, IAsyncDomainEventHandler<IAsyncDomainEvent>
    {
        public Task Handle(IDomainEvent notification, CancellationToken cancellationToken)
        {
            Log.Debug("Domain event {EventType} recorded: {@Event}.", notification.GetType().Name, notification);

            return Task.CompletedTask;
        }

        public Task Handle(IAsyncDomainEvent notification, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                Log.Debug("Async Domain event {EventType} recorded: {@Event}.", notification.GetType().Name, notification);
            }
            );
        }
    }
}