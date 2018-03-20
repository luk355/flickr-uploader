using MediatR;

namespace FlickrUploader.Core.Eventing
{
    public interface IDomainEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IDomainEvent
    {
    }
}