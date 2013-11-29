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

        public void DeleteFile(string file)
        {
            File.Delete(file);
        }


        public void MoveFile(string sourceFile, string destinationFile)
        {
            File.Move(sourceFile, destinationFile);
        }


        public bool DirectoryExists(string folder)
        {
            return Directory.Exists(folder);
        }

        public void CreateDirectory(string folder)
        {
            Directory.CreateDirectory(folder);
        }
    }
}
