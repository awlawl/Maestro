﻿using System.Collections.Generic;

namespace MusicData
{
    public interface ILibraryRepository
    {
        void AddMusicToLibrary(MusicInfo[] song);
        List<MusicInfo> GetAllMusic();
        void AddDirectoryToLibrary(string directoryPath);
        void ClearLibrary();
        List<MusicInfo> SearchLibrary(string term);
        MusicInfo GetSongById(string id);
    }
}
