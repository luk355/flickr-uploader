using FlickrUploader.Business.Features.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlickrUploader.Console
{
    class FunkyTestPersistantStorage : IPersistentStorage
    {
        public string LoadValue(string key)
        {
            return string.Empty;
        }

        public void PersistValue(string key, string value)
        {
            System.Console.WriteLine($"Key: {key}, value: {value}");
        }
    }
}
