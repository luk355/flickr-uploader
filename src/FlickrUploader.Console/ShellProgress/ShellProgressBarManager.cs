using Serilog;
using ShellProgressBar;
using System;
using System.Collections.Generic;

namespace FlickrUploader.Console.ShellProgress
{
    public class ShellProgressBarManager
    {
        private Dictionary<string, ProgressBar> progressBars;

        public ShellProgressBarManager()
        {
            progressBars = new Dictionary<string, ProgressBar>();
        }

        public void StartNewProgressBar(string folderPath, int filesToUpload)
        {
            var options = new ProgressBarOptions
            {
                ForegroundColor = ConsoleColor.Green,
                ForegroundColorDone = ConsoleColor.DarkBlue,
                BackgroundColor = ConsoleColor.DarkGray,
                BackgroundCharacter = '\u2593'
                //ProgressCharacter = '─',
                //ProgressBarOnBottom = true
            };

            var pbar = new ProgressBar(filesToUpload, $"Uploading images from '{folderPath}'.", options);

            progressBars.Add(folderPath, pbar);
        }

        public void RecordProgress(string folderPath, string fileUploaded)
        {
            if (!progressBars.ContainsKey(folderPath))
            {
                Log.Warning("Progress bar with {Path} does not exist!", folderPath);
                return;
            }

            var pbar = progressBars[folderPath];

            pbar.Tick($"Uploaded {pbar.CurrentTick + 1} of {pbar.MaxTicks} from '{folderPath}': '{fileUploaded}' file uploaded.");
        }

        public void RemoveProgressBar(string folderPath)
        {
            if (!progressBars.ContainsKey(folderPath))
            {
                Log.Warning("Progress bar with {Path} does not exist!", folderPath);
                return;
            }

            progressBars[folderPath].Dispose();
        }
    }
}
