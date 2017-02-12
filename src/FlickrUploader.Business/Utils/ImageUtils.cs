using MetadataExtractor;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FlickrUploader.Business.Extensions
{
    public static class ImageUtils
    {
        public static IList<string> GetKeywords(Stream stream)
        {
            IList<string> keywords = null;

            try
            {
                var directories = ImageMetadataReader.ReadMetadata(stream);

                var iptc = directories.OfType<MetadataExtractor.Formats.Iptc.IptcDirectory>().FirstOrDefault();

                keywords = iptc.GetKeywords();
            }
            catch (Exception e)
            {
                Log.Error(e, "Failed to get keywords");
            }

            if (keywords == null)
            {
                keywords = new List<string>();
            }

            return keywords;
        }
    }
}
