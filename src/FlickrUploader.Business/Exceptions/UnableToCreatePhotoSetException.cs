using System;

namespace FlickrUploader.Business.Exceptions
{
    public class UnableToCreatePhotoSetException : Exception
    {
        public UnableToCreatePhotoSetException(string message) : base(message)
        {
        }
    }
}