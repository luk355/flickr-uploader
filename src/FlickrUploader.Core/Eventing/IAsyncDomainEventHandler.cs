using MediatR;

namespace FlickrUploader.Core.Eventing
{
    public interface IAsyncDomainEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IAsyncDomainEvent
    {
    }
}