
using System.Collections.Generic;
using MusicData;

namespace Tests
{
    public class DummyPlaylistWatcher : IPlaylistWatcher
    {
        public List<string> PlayHistory { get; set; }

        public DummyPlaylistWatcher()
        {
            PlayHistory = new List<string>();
        }

        public void AttachToPlaylist(Playlist playlist)
        {
            throw new System.NotImplementedException();
        }

        public void PlaySong(string filename)
        {
            PlayHistory.Add(filename);
        }

        
    }
}
