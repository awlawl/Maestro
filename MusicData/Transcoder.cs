using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Dynamic;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MusicData
{
    public class Transcoder : ITranscoder
    {
        private string _pathToFFMPEG = "";

        public Transcoder()
        {
            _pathToFFMPEG = System.Configuration.ConfigurationManager.AppSettings["PathToFFMPEG"];
        }

        public Transcoder(string pathToFFMPEG)
        {
            _pathToFFMPEG = pathToFFMPEG;
        }

        public string Transcode(string inputFile)
        {
            Log.Debug("Transcoding " + inputFile);
            var cmd = GetCommandLine(inputFile);
            File.Delete(cmd.OutputPath);

            Process process = new Process();
           
            process.StartInfo.FileName = cmd.FileName;
            process.StartInfo.Arguments = cmd.Arguments;
            process.Start();
            process.WaitForExit();

            Log.Debug("Done Transcoding " + cmd.OutputPath);

            return cmd.OutputPath;
        }

        public dynamic GetCommandLine(string inputFile)
        {
            var newFilePath = inputFile.Replace(Path.GetExtension(inputFile), ".mp3");
            dynamic result = new ExpandoObject();
            result.FileName = _pathToFFMPEG;
            result.Arguments = "-i \"" + inputFile + "\" -acodec mp3 -ac 2 -ab 160k \"" + newFilePath + "\"";
            result.OutputPath = newFilePath;

            return result;
        }
    }
}
