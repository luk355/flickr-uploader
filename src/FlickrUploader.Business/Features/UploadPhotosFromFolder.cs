using System.IO.Abstractions;
using System.Linq;
using MediatR;
using Serilog;
using UnifiedMediatR.Mediator;
using FlickrUploader.Core.Eventing;
using System.IO;

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

                var photosetName = dirInfo.Name;

                if (!dirInfo.Exists)
                {
                    Log.Error("Folder {FolderName} does not exist!", message.FolderPath);
                    return Unit.Value;
                }

                var photosToUpload = dirInfo.EnumerateFiles("*.jpg").ToList();

                if (!photosToUpload.Any())
                {
                    Log.Debug("No photos found in {PhotoFolder}", message.FolderPath);
                    return Unit.Value;
                }

                // remove any already already uploaded photos - checking by photo title at the moment
                var photosetId = _flickrClient.PhotosetsGetList().FirstOrDefault(x => x.Title == photosetName)?.PhotosetId;

                if (photosetId != null)
                {
                    var photosInPhotoset = _flickrClient.GetPhotoNamesInPhotoset(photosetId);
                    Log.Information("Album already exists in Flickr with {Photos}. These are not being uploaded again.", photosInPhotoset);

                    photosToUpload.RemoveAll(x => photosInPhotoset.Contains(Path.GetFileNameWithoutExtension(x.FullName)));
                }

                // upload photos one by one
                Log.Information("Uploading {PhotoCount} photos located in {Folder} folder. {Photos}", photosToUpload.Count, message.FolderPath, photosToUpload.Select(x => x.Name));
                foreach (var photo in photosToUpload)
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