using UnifiedMediatR.Eventing;

namespace FlickrUploader.Core.Eventing
{
    public interface IDomainEvent : IDomainEvent<string>
    {
        
    }
}