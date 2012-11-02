
using System.Collections.Generic;

namespace MusicData
{
    public class Playlist : Queue<MusicInfo>
    {
        public IPlaylistWatcher[] PlaylistWatcher { get; set; }
        private Stack<MusicInfo> _pastSongs = null; 

        public Playlist(IPlaylistWatcher watcher)
        {
            PlaylistWatcher = new IPlaylistWatcher[] { watcher};
            _pastSongs = new Stack<MusicInfo>();
        }

        public Playlist(IPlaylistWatcher[] watcher)
        {
            PlaylistWatcher = watcher;
            _pastSongs = new Stack<MusicInfo>();
        }

        public MusicInfo GetNextSong()
        {
            MusicInfo nextSong = this.Dequeue();
            foreach (var playlistWatcher in PlaylistWatcher)
                playlistWatcher.PlaySong(nextSong);

            _pastSongs.Push(nextSong);
            return nextSong;
        }

        public MusicInfo GetLastSong()
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
