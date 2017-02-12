using MediatR;
using UnifiedMediatR.Mediator;

namespace FlickrUploader.Business.Commands.Flickr
{
    public static class SendAuthenticationRequest
    {
        public class Command : ICommand
        {
        }

        public class AuthAggregate : ICommandHandler<Command>
        {
            private readonly IFlickrClient _flickrClient;

            public AuthAggregate(IFlickrClient flickrClient)
            {
                _flickrClient = flickrClient;
            }
            public Unit Handle(Command message)
            {
                _flickrClient.SendAuthenticationRequest();

                return Unit.Value;
            }
        }
    }
}