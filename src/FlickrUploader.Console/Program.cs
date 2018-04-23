using FlickrUploader.Console.DependencyResolution;
using Serilog;
using StructureMap;
using System;

namespace FlickrUploader.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Startup.Configure();

                var container = Container.For<ConsoleRegistry>();

                container.AssertConfigurationIsValid();
                Log.Information("Container has {@ContainerContent}", container.WhatDoIHave());

                var app = container.GetInstance<Application>();
                app.Run().Wait();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error!");
                System.Console.ReadKey();
            }
        }
    }
}
