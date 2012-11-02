
using System.Collections.Generic;

namespace MusicData
{
    public class LoopingPlaylistWatcher : IPlaylistWatcher
    {
        private Queue<MusicInfo> _loop = null;
        private Playlist _playlist = null;
 
        public LoopingPlaylistWatcher()
        {
            _loop = new Queue<MusicInfo>();
        }

        public void AddToLoop(MusicInfo song)
        {
            _loop.Enqueue(song);
        }

        public void AttachToPlaylist(Playlist playlist, ILibraryRepository library)
        {
            _playlist = playlist;
            _playlist.Clear();

            List<MusicInfo> songs = library.GetAllMusic();

            foreach (var song in songs)
                _loop.Enqueue(song);

            for (int i = 0; i < _loop.Count; i++)
                PutNextSongInThePlaylist();
        }

        public void PlaySong(MusicInfo filename)
        {
            PutNextSongInThePlaylist();
        }

        public void PutNextSongInThePlaylist()
        {
            var nextSong = _loop.Dequeue();
            _playlist.Enqueue(nextSong);
            _loop.Enqueue(nextSong);
        }
    }
}
