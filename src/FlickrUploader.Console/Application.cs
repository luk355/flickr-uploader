using FlickrUploader.Business.Commands;
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

            string code = System.Console.ReadLine();

            _mediator.Execute(new CompleteAutenticationCommand(code));

            _mediator.Query(new GetPhotosetIdByName("test"));
        }
    }
}