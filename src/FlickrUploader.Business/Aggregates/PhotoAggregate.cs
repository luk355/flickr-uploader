using FlickrNet;
using FlickrUploader.Business.Commands;
using MediatR;
using UnifiedMediatR.Mediator;

namespace FlickrUploader.Business.Aggregates
{
    public class PhotoAggregate : ICommandHandler<UploadPhotoCommand>
    {
        private readonly IFlickrClient _flickrClient;

        public PhotoAggregate(IFlickrClient flickrClient)
        {
            _flickrClient = flickrClient;
        }

        public Unit Handle(UploadPhotoCommand message)
        {
            var flickr = new Flickr();

            // upload photo

            // add photo to photo set

            throw new System.NotImplementedException();
        }
    }
}
