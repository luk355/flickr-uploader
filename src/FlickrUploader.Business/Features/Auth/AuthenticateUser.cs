using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using UnifiedMediatR.Mediator;
using FlickrUploader.Business.Commands.Flickr;

namespace FlickrUploader.Business.Features.Auth
{
    public static class AuthenticateUser
    {
        public class Command : ICommand
        {
        }

        public class CommandHandler : ICommandHandler<Command>
        {
            private readonly IFlickrClient _flickrClient;
            private readonly IUnifiedMediator<string> _mediator;
            private readonly IAuthCodeProvider _authCodeProvider;

            public CommandHandler(IUnifiedMediator<string> mediator, IFlickrClient flickrClient, IAuthCodeProvider authCodeProvider)
            {
                _mediator = mediator;
                _flickrClient = flickrClient;
                _authCodeProvider = authCodeProvider;
            }

            Unit IRequestHandler<Command, Unit>.Handle(Command message)
            {
                // checks cookie storage and configures flickrClient
                // TODO cookie stuff!

                // if this does not work authorise again
                // send auth request
                _mediator.Execute(new SendAuthenticationRequest.Command());

                // get auth key
                string code = _authCodeProvider.GetCode();

                // complete auth
                _mediator.Execute(new CompleteAutentication.Command(code));

                return Unit.Value;
            }
        }
    }
}
