using FlickrUploader.Business.Commands.Flickr;
using MediatR;
using UnifiedMediatR.Mediator;

namespace FlickrUploader.Business.Aggregates
{
    public class AuthAggregate : ICommandHandler<SendAuthenticationRequestCommand>, ICommandHandler<CompleteAutenticationCommand>
    {
        private readonly IFlickrClient _flickrClient;

        public AuthAggregate(IFlickrClient flickrClient)
        {
            _flickrClient = flickrClient;
        }
        public Unit Handle(SendAuthenticationRequestCommand message)
        {
            _flickrClient.SendAuthenticationRequest();

            return Unit.Value;
        }

        public Unit Handle(CompleteAutenticationCommand message)
        {
            _flickrClient.CompleteAutentication(message.Code);

            return Unit.Value;
        }
    }
}