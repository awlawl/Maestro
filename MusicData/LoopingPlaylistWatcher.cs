using System.Linq;
using System.Collections.Generic;

namespace MusicData
{
    public class LoopingPlaylistWatcher : IPlaylistWatcher
    {
        private Playlist _playlist = null;

        public void AttachToPlaylist(Playlist playlist, ILibraryRepository library)
        {
            _playlist = playlist;
        }

        public void SongStarting(MusicInfo song)
        {
            
        }

        public void SongEnding(MusicInfo song)
        {
            _playlist.Enqueue(song);
        }

        
    }
}
