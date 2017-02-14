using System.Threading.Tasks;
using FlickrUploader.Business.Commands;
using Serilog;
using UnifiedMediatR.Mediator;
using FlickrUploader.Business.Features.Auth;

namespace FlickrUploader.Console
{
    public class Application
    {
        private readonly IUnifiedMediator<string> _mediator;

        public Application(IUnifiedMediator<string> mediator)
        {
            _mediator = mediator;
        }

        public Task Run()
        {
            return Task.Factory.StartNew(() =>
            {
                _mediator.Execute(new AuthenticateUser.Command());

                _mediator.Execute(new UploadFolder.Command() {Path = ApplicationSettings.PhotoPath});

                Log.Information("All done! Pres any key to end the application :)");
                System.Console.ReadKey();
            });
        }
    }
}