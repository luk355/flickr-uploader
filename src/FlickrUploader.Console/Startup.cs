using Serilog;

namespace FlickrUploader.Console
{
    public class Startup
    {
        public static void Configure()
        {
            ConfigureLogging();
        }

        private static void ConfigureLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.ColoredConsole()
                .CreateLogger();
        }
    }
}