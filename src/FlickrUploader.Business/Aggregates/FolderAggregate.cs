﻿using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using FlickrUploader.Business.Commands;
using MediatR;
using Serilog;
using UnifiedMediatR.Mediator;
using FlickrUploader.Business.Extensions;

namespace FlickrUploader.Business.Aggregates
{
    public class FolderAggregate : IAsyncCommandHandler<UploadFolderCommand>
    {
        private readonly IFlickrClient _flickrClient;
        private readonly IFileSystem _fileSystem;
        private readonly IUnifiedMediator<string> _mediator;

        private const string ProcessedFileName = ".processed";

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

                if (!dirInfo.HasWritePermisssion())
                {
                    Log.Error("Missing write rights to folder {FolderName}!", message.Path);
                    return Unit.Value;
                }

                List<Task> tasks = new List<Task>();

                // upload photos from all sub-folders
                foreach (var directory in dirInfo.EnumerateDirectories())
                {
                    tasks.Add(_mediator.ExecuteAsync(new UploadFolderCommand() { Path = directory.FullName }));
                }

                if (dirInfo.EnumerateFiles(ProcessedFileName).Any())
                {
                    Log.Information("Folder {FolderName} has been already processed. Skipping...", message.Path);
                    return Unit.Value;
                }

                // Upload all photos in folder
                tasks.Add(_mediator.ExecuteAsync(new UploadPhotosFromFolderCommand() { FolderPath = message.Path }));

                Task.WaitAll(tasks.ToArray());

                // creates processed file in folder
                var processedFile = new FileInfo(Path.Combine(dirInfo.FullName, ProcessedFileName));
                processedFile.Create().Close();

                Log.Information("Upload of all photos within folder {FolderPath} is finished!", message.Path);

                return Unit.Value;
            });
        }
    }
}