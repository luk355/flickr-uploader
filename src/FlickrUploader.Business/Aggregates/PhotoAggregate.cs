using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO.Abstractions;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using FlickrNet;
using FlickrUploader.Business.Commands;
using FlickrUploader.Business.Queries;
using MediatR;
using Serilog;
using UnifiedMediatR.Mediator;

namespace FlickrUploader.Business.Aggregates
{
    public class PhotoAggregate : ICommandHandler<UploadPhotoCommand>, IAsyncCommandHandler<UploadFolderCommand>, IAsyncCommandHandler<UploadPhotosFromFolderCommand>
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

        public Unit Handle(UploadPhotoCommand message)
        {
            var flickr = new Flickr();

            // upload photo

            // add photo to photo set

            throw new System.NotImplementedException();
        }

        public Task<Unit> Handle(UploadPhotosFromFolderCommand message)
        {
            return Task<Unit>.Factory.StartNew(() =>
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
                    Log.Information("No photos find in {PhotoFolder}", message.FolderPath);
                    return Unit.Value;
                }

                // get photoSet
                var photosetName = dirInfo.Name;


                var photoSetId = _mediator.Query(new GetPhotosetIdByName(photosetName));

                // create photoset if not exists
                if (string.IsNullOrEmpty(photoSetId))
                {
                    photosetName = _mediator.Execute(new CreatePhotosetCommand(photosetName));
                }

                if (string.IsNullOrEmpty(photoSetId))
                {
                    throw new UnableToCreatePhotoSetException($"Unable to create {photosetName} photoset.");
                }

                // upload photos one by one
                throw new NotImplementedException();

                return Unit.Value;
            });
        }

        Task<Unit> IAsyncRequestHandler<UploadFolderCommand, Unit>.Handle(UploadFolderCommand message)
        {
            return Task<Unit>.Factory.StartNew(() =>
            {
                var dirInfo = _fileSystem.DirectoryInfo.FromDirectoryName(message.Path);

                if (!dirInfo.Exists)
                {
                    Log.Error("Folder {FolderName} does not exist!", message.Path);
                    return Unit.Value;
                }

                List<Task> tasks = new List<Task>();

                // Upload all photos in folder
                tasks.Add(_mediator.ExecuteAsync(new UploadPhotosFromFolderCommand() { FolderPath = message.Path }));

                // Upload photos for all subfolders
                foreach (var directory in dirInfo.EnumerateDirectories())
                {
                    tasks.Add(_mediator.ExecuteAsync(new UploadFolderCommand() { Path = directory.FullName }));
                }

                Task.WaitAll(tasks.ToArray());
                return Unit.Value;
            });
        }
    }

    public class UnableToCreatePhotoSetException : Exception
    {
        public UnableToCreatePhotoSetException(string message) : base(message)
        {
        }
    }
}
