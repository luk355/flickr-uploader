using System.IO.Abstractions;
using FlickrUploader.Business;
using FlickrUploader.Business.Commands;
using MediatR;
using StructureMap;
using UnifiedMediatR.Mediator;
using FlickrUploader.Business.Features.Auth;

namespace FlickrUploader.Console.DependencyResolution
{
    public class ConsoleRegistry : Registry
    {
        public ConsoleRegistry()
        {
            // MediatR
            Scan(scanner =>
            {
                scanner.AssemblyContainingType<CreatePhotoset.Command>(); // bussiness assembly
                scanner.TheCallingAssembly(); // this assembly
                scanner.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
                scanner.ConnectImplementationsToTypesClosing(typeof(IAsyncRequestHandler<,>));
                scanner.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));
                scanner.ConnectImplementationsToTypesClosing(typeof(IAsyncNotificationHandler<>));
            });
            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));
            For<IMediator>().Use<Mediator>();
            For<IUnifiedMediator<string>>().Use<UnifiedMediator<string>>();

            For<IFlickrClient>().Use<FlickrClient>()
                .Ctor<string>("apiKey").Is(ApplicationSettings.Flickr.ApiKey)
                .Ctor<string>("secret").Is(ApplicationSettings.Flickr.Secret)
                .Singleton();

            For<IFileSystem>().Use<FileSystem>();
            For<IAuthCodeProvider>().Use<ConsoleAuthCodeProvider>();
            For<IPersistentStorage>().Use<JsonFilePersistantStorage>();
        }
    }
}