using FlickrUploader.Console.DependencyResolution;
using Serilog;
using StructureMap;
using System;
using System.Threading.Tasks;

namespace FlickrUploader.Console
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                Startup.Configure();

                var container = Container.For<ConsoleRegistry>();

                container.AssertConfigurationIsValid();
                Log.Information("Container has {@ContainerContent}", container.WhatDoIHave());

                var app = container.GetInstance<Application>();
                await app.Run();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error!");
                System.Console.ReadKey();
            }
        }
    }
}
