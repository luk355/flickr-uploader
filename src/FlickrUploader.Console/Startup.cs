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
            LoadAppSettings();
            ConfigureLogging();
        }

        private static void ConfigureLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .WriteTo.RollingFile("log-{Date}.log", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss,fff} [{Level:u4}] {Message}{NewLine}{Exception}")
                .CreateLogger();
        }

        private static void LoadAppSettings()
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables();

            Configuration = builder.Build();

        }
    }
}