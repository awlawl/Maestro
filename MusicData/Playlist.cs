
using System.Collections.Generic;

namespace MusicData
{
    public class Playlist : Queue<string>
    {
        private IPlaylistWatcher _playlistWatcher = null;
        private Stack<string> _pastSongs = null; 

        public Playlist(IPlaylistWatcher watcher)
        {
            _playlistWatcher = watcher;
            _pastSongs = new Stack<string>();
        }

        public string GetNextSong()
        {
            string nextSong = this.Dequeue();
            _playlistWatcher.PlaySong(nextSong);
            _pastSongs.Push(nextSong);
            return nextSong;
        }

        public string GetLastSong()
        {
            return _pastSongs.Peek();
        }
        
        public bool AreSongsAvailable()
        {
            return this.Count > 0;
        }

        public bool HistoryIsAvailable()
        {
            return _pastSongs.Count > 0;
        }
    }
}
