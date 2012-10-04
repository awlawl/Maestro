﻿using System.Collections.Generic; 

namespace MusicData
{
    public class MemoryLibraryRepository : ILibraryRepository
    {
        private static List<MusicInfo> _library = new List<MusicInfo>();

        public void AddMusicToLibrary(MusicInfo[] song)
        {
            _library.AddRange(song);
        }

        public List<MusicInfo> GetAllMusic()
        {
            var result = new List<MusicInfo>();

            result.AddRange(_library);

            return result;
        }

        public void AddDirectoryToLibrary(string directoryPath)
        {
            var musicInfoReader = new MusicInfoReader();

            List<MusicInfo> allMusic = musicInfoReader.CrawlDirectory(directoryPath);

            this.AddMusicToLibrary(allMusic.ToArray());
        }

        public void ClearLibrary()
        {
            _library.Clear();
        }
    }
}
