
using System.Collections.Generic;

namespace MusicData
{
    public class Playlist : Queue<string>
    {
        private IPlaylistWatcher _playlistWatcher = null;

        public Playlist(IPlaylistWatcher watcher)
        {
            _playlistWatcher = watcher;
        }

        public string GetNextSong()
        {
            string nextSong = this.Dequeue();
            _playlistWatcher.PlaySong(nextSong);

            return nextSong;
        }



        public bool AreSongsAvailable()
        {
            return this.Count > 0;
        }
    }
}
