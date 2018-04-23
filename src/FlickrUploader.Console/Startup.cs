using Serilog;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace FlickrUploader.Console
{
    public class Startup
    {
        static public IConfigurationRoot Configuration { get; set; }

        public static void Configure()
        {
            ConfigureLogging();
            LoadAppSettings();
        }

        private static void ConfigureLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.ColoredConsole()
                .WriteTo.RollingFile("log-{Date}.log", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss,fff} [{Level:u4}] {Message}{NewLine}{Exception}")
                .CreateLogger();
        }

        private static void LoadAppSettings()
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

        }
    }
}