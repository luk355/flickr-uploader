using System;
using FlickrUploader.Business.Commands;
using MediatR;
using UnifiedMediatR.Mediator;

namespace FlickrUploader.Business.Aggregates
{
    public class PhotosetAggregate : ICommandHandler<CreatePhotosetCommand, string>
    {
        private readonly IFlickrClient _flickrClient;

        public PhotosetAggregate(IFlickrClient flickrClient)
        {
            _flickrClient = flickrClient;
        }

        public string Handle(CreatePhotosetCommand message)
        {
            throw new NotImplementedException();
        }
    }
}
