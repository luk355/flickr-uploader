namespace FlickrUploader.Console
{
    public static class ApplicationSettings
    {
        public static class Flickr
        {
            // TODO load from config
            public static string ApiKey => Startup.Configuration["flickrApiKey"];

            public static string Secret => Startup.Configuration["flickrSecret"];
        }

        public static string PhotoPath => Startup.Configuration["photoPath"];
    }
}