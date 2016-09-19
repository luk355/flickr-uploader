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
            _mediator.Query(new GetPhotosetIdByName("test"));
        }
    }
}