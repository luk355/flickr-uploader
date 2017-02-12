using System.IO.Abstractions;
using System.Linq;
using MediatR;
using Serilog;
using UnifiedMediatR.Mediator;
using FlickrUploader.Core.Eventing;

namespace FlickrUploader.Business.Commands
{
    public static class UploadPhotosFromFolder
    {

        public class Command : ICommand
        {
            public string FolderPath { get; set; }
        }

        public class CommandHandler : ICommandHandler<Command>
        {
            private readonly IFlickrClient _flickrClient;
            private readonly IFileSystem _fileSystem;
            private readonly IUnifiedMediator<string> _mediator;

            public CommandHandler(IFlickrClient flickrClient, IFileSystem fileSystem, IUnifiedMediator<string> mediator)
            {
                _flickrClient = flickrClient;
                _fileSystem = fileSystem;
                _mediator = mediator;
            }



            Unit IRequestHandler<Command, Unit>.Handle(Command message)
            {
                var dirInfo = _fileSystem.DirectoryInfo.FromDirectoryName(message.FolderPath);

                if (!dirInfo.Exists)
                {
                    Log.Error("Folder {FolderName} does not exist!", message.FolderPath);
                    return Unit.Value;
                }

                var photos = dirInfo.EnumerateFiles("*.jpg").ToList();

                if (!photos.Any())
                {
                    Log.Information("No photos found in {PhotoFolder}", message.FolderPath);
                    return Unit.Value;
                }

                // get photoSet
                var photosetName = dirInfo.Name;

                // upload photos one by one

                Log.Information("Uploading {Photos} photos located in {Folder} folder.", photos.Select(x => x.Name), message.FolderPath);
                foreach (var photo in photos)
                {
                    _mediator.Execute(new UploadPhoto.Command() { Path = photo.FullName, PhotosetName = photosetName });
                }

                _mediator.Publish(new PhotosFromFolderUploadedEvent() { Id = message.FolderPath });

                return Unit.Value;
            }
        }

        public class PhotosFromFolderUploadedEvent : IDomainEvent
        {
            public string Id { get; set; }
        }

    }
}