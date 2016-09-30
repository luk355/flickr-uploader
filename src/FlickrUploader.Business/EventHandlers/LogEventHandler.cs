using System;
using System.Threading.Tasks;
using FlickrUploader.Core.Eventing;
using MediatR;
using Serilog;

namespace FlickrUploader.Business.EventHandlers
{
    public class LogEventHandler : IAsyncDomainEventHandler<IAsyncDomainEvent>, IDomainEventHandler<IDomainEvent>
    {
        public Task Handle(IAsyncDomainEvent notification)
        {
            return Task.Factory.StartNew(() =>
                {
                    Log.Debug("Async Domain event {EventType} recorded: {@Event}.", notification.GetType().Name, notification);
                }
            );
        }

        void INotificationHandler<IDomainEvent>.Handle(IDomainEvent notification)
        {
            Log.Debug("Domain event {EventType} recorded: {@Event}.", notification.GetType().Name, notification);
        }
    }
}