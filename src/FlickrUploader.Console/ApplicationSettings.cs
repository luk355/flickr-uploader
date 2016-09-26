using System.Runtime.CompilerServices;

namespace FlickrUploader.Console
{
    public static class ApplicationSettings
    {
        public static class Flickr
        {
            // TODO load from config
            public static string ApiKey => "3c520555f1a86a15577ba8923473b707";
            public static string Secret => "9cf5cdb3d0498139";
        }
    }
}
