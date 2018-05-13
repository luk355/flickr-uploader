using System.Threading.Tasks;
using FlickrUploader.Business.Commands;
using Serilog;
using FlickrUploader.Business.Features.Auth;
using FlickrUploader.Core.Mediator;

namespace FlickrUploader.Console
{
    public class Application
    {
        private readonly IUnifiedMediator _mediator;

        public Application(IUnifiedMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Run()
        {
            // 1) Authenticate
            // 2) Init - upload all photos not in photosets already
            // 3) Watch - upload any newly added photos while app is running - TODO

            await _mediator.Execute(new AuthenticateUser.Command());

            Log.Information("Starting image upload from {Folder} folder.", ApplicationSettings.PhotoPath);

            await _mediator.Execute(new UploadFolder.Command() { Path = ApplicationSettings.PhotoPath });

            Log.Information("All done! Pres any key to end the application :)");
            System.Console.ReadKey();
        }
    }
}