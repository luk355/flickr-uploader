using UnifiedMediatR.Eventing;

namespace FlickrUploader.Core.Eventing
{
    public interface IAsyncDomainEvent : IAsyncDomainEvent<string>
    {
        
    }
}