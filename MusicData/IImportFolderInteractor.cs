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
        string MoveToLibraryFolder(string file, string artist, string album);
    }
}
