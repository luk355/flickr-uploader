using MediatR;
using UnifiedMediatR.Mediator;

namespace FlickrUploader.Business.Commands.Flickr
{
    public static class CompleteAutentication
    {
        public class Command : ICommand
        {
            public string Code { get; set; }

            public Command(string code)
            {
                Code = code;
            }
        }

        public class CommandHandler : ICommandHandler<Command>
        {
            private readonly IFlickrClient _flickrClient;

            public CommandHandler(IFlickrClient flickrClient)
            {
                _flickrClient = flickrClient;
            }
   
            public Unit Handle(Command message)
            {
                _flickrClient.CompleteAutentication(message.Code);

                return Unit.Value;
            }
        }
    }
}