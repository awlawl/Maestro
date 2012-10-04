
using System.Collections.Generic;

namespace MusicData
{
    public class Playlist : Queue<string>
    {
        public IPlaylistWatcher PlaylistWatcher { get; set; }
        private Stack<string> _pastSongs = null; 

        public Playlist(IPlaylistWatcher watcher)
        {
            PlaylistWatcher = watcher;
            _pastSongs = new Stack<string>();
        }

        public string GetNextSong()
        {
            string nextSong = this.Dequeue();
            PlaylistWatcher.PlaySong(nextSong);
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
