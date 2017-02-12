using System;
using MediatR;
using UnifiedMediatR.Mediator;
using System.Linq;
using FlickrUploader.Business.Exceptions;
using FlickrUploader.Core.Eventing;

namespace FlickrUploader.Business.Commands
{
    public static class AddPhotoToPhotoset
    {
        public class Command : ICommand
        {
            public string PhotoId { get; set; }
            public string PhotosetName { get; set; }
        }

        public class CommandHandler : ICommandHandler<Command>
        {
            private readonly IFlickrClient _flickrClient;
            private readonly IUnifiedMediator<string> _mediator;

            public CommandHandler(IFlickrClient flickrClient, IUnifiedMediator<string> mediator)
            {
                _flickrClient = flickrClient;
                _mediator = mediator;
            }

            public Unit Handle(Command message)
            {
                var photosetId =
                _flickrClient.PhotosetsGetList().FirstOrDefault(x => x.Title == message.PhotosetName)?.PhotosetId;

                // create photoset if not exists
                // this adds already photoToPhotoset -> no additional photo addtion needed
                if (string.IsNullOrEmpty(photosetId))
                {
                    photosetId = _mediator.Execute(new CreatePhotoset.Command(message.PhotosetName, message.PhotoId));

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

        public class PhotoAddedAsMainToPhotosetEvent : PhotoAddedToPhotosetEvent
        {
        }

        public class PhotoAddedToPhotosetEvent : IDomainEvent
        {
            public string Id { get; set; }

            public string PhotosetId { get; set; }
        }
    }
}