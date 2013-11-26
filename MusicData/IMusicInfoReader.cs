using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicData
{
    public interface IMusicInfoReader
    {
        MusicInfo GetInfoForFile(string fullPath);
    }
}
