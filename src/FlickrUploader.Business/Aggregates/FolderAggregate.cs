using System.Collections.Generic;
using System.IO.Abstractions;
using System.Threading.Tasks;
using FlickrUploader.Business.Commands;
using MediatR;
using Serilog;
using UnifiedMediatR.Mediator;

namespace FlickrUploader.Business.Aggregates
{
    public class FolderAggregate : IAsyncCommandHandler<UploadFolderCommand>
    {
        private readonly IFlickrClient _flickrClient;
        private readonly IFileSystem _fileSystem;
        private readonly IUnifiedMediator<string> _mediator;

        public FolderAggregate(IFlickrClient flickrClient, IFileSystem fileSystem, IUnifiedMediator<string> mediator)
        {
            _flickrClient = flickrClient;
            _fileSystem = fileSystem;
            _mediator = mediator;
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

                // upload photos from all sub-folders
                foreach (var directory in dirInfo.EnumerateDirectories())
                {
                    tasks.Add(_mediator.ExecuteAsync(new UploadFolderCommand() { Path = directory.FullName }));
                }

                Task.WaitAll(tasks.ToArray());

                Log.Information("Upload of all photos within folder {FolderPath} is finished!", message.Path);

                return Unit.Value;
            });
        }
    }
}