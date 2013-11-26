using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MusicData
{
    public class ImportFolderInteractor : IImportFolderInteractor
    {
        public string[] GetFilesForFolder(string folder)
        {
            return Directory.GetFiles(folder);
        }

        public string MoveToLibraryFolder(string file, string artist, string album)
        {
            var root = System.Configuration.ConfigurationManager.AppSettings["LibraryRootFolder"];
            var artistPath = root + "\\" + artist;
            var albumPath = artistPath + "\\" + album;
            var fullPath  = albumPath + "\\" + Path.GetFileName(file);

            if (!Directory.Exists(artistPath))
                Directory.CreateDirectory(artistPath);

            if (!Directory.Exists(albumPath))
                Directory.CreateDirectory(albumPath);

            File.Move(file, fullPath);
            return fullPath;
        }
    }
}
