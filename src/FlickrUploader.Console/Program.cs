using System;
using FlickrUploader.Console.DependencyResolution;
using Serilog;
using StructureMap;

namespace FlickrUploader.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Startup.Configure();

                var container = Container.For<ConsoleRegistry>();

                container.AssertConfigurationIsValid();

                var app = container.GetInstance<Application>();
                app.Run().Wait();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error!");
            }
        }

    }
}
