using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicData
{
    public interface IImportFolderInteractor
    {
        string[] GetFilesForFolder(string folder);
        void MoveFile(string sourceFile, string destinationFile);
        void DeleteFile(string file);
        bool DirectoryExists(string folder);
        void CreateDirectory(string folder);
    }
}
