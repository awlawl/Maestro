using System.Collections.Generic; 

namespace MusicData
{
    public class MemoryLibraryRepository : ILibraryRepository
    {
        private static List<MusicInfo> _library = new List<MusicInfo>();
        private IRealTimeMessaging _messaging = null;

        public MemoryLibraryRepository()
        {
        }

        public MemoryLibraryRepository(IRealTimeMessaging messaging)
        {
            _messaging = messaging;
        }

        public void AddMusicToLibrary(MusicInfo[] song)
        {
            _library.AddRange(song);

            if (_messaging != null)
                _messaging.SendSongsAdded(song);
            
                
        }

        public void AddOrUpdateMusicInLibrary(MusicInfo song)
        {
            _library.Add(song);

            if (_messaging != null)
                _messaging.SendSongsAdded(new MusicInfo[] {song});
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


        public List<MusicInfo> SearchLibrary(string term)
        {
            throw new System.NotImplementedException();
        }

        public MusicInfo GetSongById(string id)
        {
            throw new System.NotImplementedException();
        }

        public List<SavedPlaylist> GetAllSavedPlaylists()
        {
            throw new System.NotImplementedException();
        }

        public List<MusicInfo> GetAllSongsForSavedPlaylist(string name)
        {
            throw new System.NotImplementedException();
        }

        public SavedPlaylist AddNewSavedPlaylist(string name)
        {
            throw new System.NotImplementedException();
        }

        public void AddSongToSavedPlaylist(string savedPlaylistName, string songId)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveSongFromSavedPlaylist(string savedPlaylistName, string songId)
        {
            throw new System.NotImplementedException();
        }


        
    }
}
