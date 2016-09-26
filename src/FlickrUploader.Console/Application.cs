﻿using FlickrUploader.Business.Commands;
using FlickrUploader.Business.Commands.Flickr;
using FlickrUploader.Business.Queries;
using UnifiedMediatR.Mediator;

namespace FlickrUploader.Console
{
    public class Application
    {
        private readonly IUnifiedMediator<string> _mediator;

        public Application(IUnifiedMediator<string> mediator)
        {
            _mediator = mediator;
        }

        public void Run()
        {
            _mediator.Execute(new SendAuthenticationRequestCommand());
            System.Console.WriteLine("Please provide authentication code: ");
            string code = System.Console.ReadLine();
            _mediator.Execute(new CompleteAutenticationCommand(code.Replace("-",string.Empty)));

            _mediator.ExecuteAsync(new UploadFolderCommand() {Path = ApplicationSettings.PhotoPath});

            _mediator.Query(new GetPhotosetIdByName("test"));
        }
    }
}