using System;
using System.Linq;
using System.Threading.Tasks;
using FlickrUploader.Business.Commands;
using FlickrUploader.Business.DomainEvents;
using FlickrUploader.Business.Queries;
using MediatR;
using UnifiedMediatR.Mediator;

namespace FlickrUploader.Business.Aggregates
{
    public class PhotosetAggregate : ICommandHandler<CreatePhotosetCommand, string>, ICommandHandler<AddPhotoToPhotosetCommand>
    {
        private readonly IFlickrClient _flickrClient;
        private readonly IUnifiedMediator<string> _mediator;

        public PhotosetAggregate(IFlickrClient flickrClient, IUnifiedMediator<string> mediator)
        {
            _flickrClient = flickrClient;
            _mediator = mediator;
        }

        public string Handle(CreatePhotosetCommand message)
        {
            return _flickrClient.CreatePhotoset(message.Name, message.PrimaryPhotoId);
        }

        public Unit Handle(AddPhotoToPhotosetCommand message)
        {
            var photosetId =
                _flickrClient.PhotosetsGetList().FirstOrDefault(x => x.Title == message.PhotosetName)?.PhotosetId;

            // create photoset if not exists
            // this adds already photoToPhotoset -> no additional photo addtion needed
            if (string.IsNullOrEmpty(photosetId))
            {
                photosetId = _mediator.Execute(new CreatePhotosetCommand(message.PhotosetName, message.PhotoId));

                _mediator.Publish(new PhotoAddedAsMainToPhotosetEvent() { Id = message.PhotoId, PhotosetId = photosetId });

                return Unit.Value;
            }

            if (string.IsNullOrEmpty(photosetId))
            {
                throw new UnableToCreatePhotoSetException($"Unable to retreive {message.PhotosetName} photoset.");
            }

            _flickrClient.AddPhotoToPhotoset(message.PhotoId, photosetId);

            _mediator.Publish(new PhotoAddedToPhotosetEvent() { Id = message.PhotoId, PhotosetId = photosetId });

            return Unit.Value;
        }
    }
}
