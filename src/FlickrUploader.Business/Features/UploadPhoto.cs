using System.IO;
using System.IO.Abstractions;
using MediatR;
using UnifiedMediatR.Mediator;
using FlickrUploader.Core.Eventing;

namespace FlickrUploader.Business.Commands
{
    public static class UploadPhoto
    {

        public class Command : ICommand
        {
            public string Path { get; set; }
            public string PhotosetName { get; set; }
        }

        public class PhotoAggregate : ICommandHandler<Command>
        {
            private readonly IFlickrClient _flickrClient;
            private readonly IFileSystem _fileSystem;
            private readonly IUnifiedMediator<string> _mediator;

            public PhotoAggregate(IFlickrClient flickrClient, IFileSystem fileSystem, IUnifiedMediator<string> mediator)
            {
                _flickrClient = flickrClient;
                _fileSystem = fileSystem;
                _mediator = mediator;
            }

            public Unit Handle(Command message)
            {
                // upload photo
                var id = _flickrClient.UploadPicture(message.Path, Path.GetFileNameWithoutExtension(message.Path));

                // add photo to photoset
                _mediator.Execute(new AddPhotoToPhotoset.Command()
                {
                    PhotoId = id,
                    PhotosetName = message.PhotosetName
                });

                _mediator.Publish(new PhotoUploadedEvent() { Id = id, PhotoSet = message.PhotosetName });

                return Unit.Value;
            }
        }

        public class PhotoUploadedEvent : IDomainEvent
        {
            public string Id { get; set; }

            public string PhotoSet { get; set; }
        }
    }
}