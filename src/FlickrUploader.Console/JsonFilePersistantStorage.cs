using FlickrUploader.Business.Features.Auth;
using Newtonsoft.Json;
using Serilog;
using System.Collections.Generic;
using System.IO.Abstractions;

namespace FlickrUploader.Console
{
    class JsonFilePersistantStorage : IPersistentStorage
    {
        private readonly IFileSystem _fileSystem;
        private readonly string fileName = "storage.json";

        public JsonFilePersistantStorage(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        

        public string LoadValue(string key)
        {
            if (!_fileSystem.File.Exists(fileName))
            {
                return null;
            }

            var text = _fileSystem.File.ReadAllText(fileName);

            var values = JsonConvert.DeserializeObject<IDictionary<string, string>>(text);

            if (values.ContainsKey(key))
            {
                return values[key];
            }

            return null;
        }

        public void PersistValue(string key, string value)
        {
            Log.Information($"Persisting Key: {key}, value: {value}");
            
            IDictionary<string, string> values = new Dictionary<string, string>();


            // TODO add lock
            if (_fileSystem.File.Exists(fileName))
            {
                var text = _fileSystem.File.ReadAllText(fileName);

                values = JsonConvert.DeserializeObject<IDictionary<string, string>>(text);
            }

            if (values.ContainsKey(key))
            {
                values[key] = value;
            } else
            {
                values.Add(key, value);
            }

            var newContent = JsonConvert.SerializeObject(values);

            _fileSystem.File.WriteAllText(fileName, newContent);
        }
    }
}
