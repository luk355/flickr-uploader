using System.Threading;
using System.Threading.Tasks;
using FlickrUploader.Core.Mediator;
using MediatR;

namespace FlickrUploader.Business.Features.Auth
{
    public static class AuthenticateUser
    {
        private const string TokenKey = "OauthToken";
        private const string SecretKey = "OauthSecret";

        public class Command : ICommand
        {
        }

        public class CommandHandler : ICommandHandler<Command>
        {
            private readonly IFlickrClient _flickrClient;
            private readonly IUnifiedMediator _mediator;
            private readonly IAuthCodeProvider _authCodeProvider;
            private readonly IPersistentStorage _persistentStorage;

            public CommandHandler(IUnifiedMediator mediator, IFlickrClient flickrClient, IAuthCodeProvider authCodeProvider, IPersistentStorage persistentStorage)
            {
                _mediator = mediator;
                _flickrClient = flickrClient;
                _authCodeProvider = authCodeProvider;
                _persistentStorage = persistentStorage;
            }

            public Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                // checks cookie storage and configures flickrClient
                // TODO cookie stuff!
                (string token, string secret) accessData = LoadOauthDataFromPersistantStorage();
                _flickrClient.SetAccessToken(accessData.token, accessData.secret);

                // has flickr access
                if (_flickrClient.IsAccessTokenValid())
                {
                    return Task.FromResult(Unit.Value);
                }

                // if this does not work authorise again

                // reset flickr access data
                _flickrClient.ResetAccessData();

                // send auth request
                _flickrClient.SendAuthenticationRequest();

                // get auth key
                string code = _authCodeProvider.GetCode();

                // complete auth
                _flickrClient.CompleteAutentication(code);

                // persist auth token to persistant storage
                var newAccessData = _flickrClient.GetAccessToken();
                PersistOauthData(newAccessData.token, newAccessData.secret);

                return Task.FromResult(Unit.Value);
            }

            private (string token, string secret) LoadOauthDataFromPersistantStorage()
            {
                var token = _persistentStorage.LoadValue(TokenKey);
                var secret = _persistentStorage.LoadValue(SecretKey);

                return (token, secret);
            }

            private void PersistOauthData(string token, string secret)
            {
                _persistentStorage.PersistValue(TokenKey, token);
                _persistentStorage.PersistValue(SecretKey, secret);
            }
        }
    }
}
