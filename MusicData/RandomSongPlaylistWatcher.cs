using System;
using System.Linq;

namespace MusicData
{
    public class RandomSongPlaylistWatcher : IPlaylistWatcher
    {
        private Playlist _playlist;
        private ILibraryRepository _library;
        private Random _random;

        private const int PLAYLIST_SIZE = 5;

        public RandomSongPlaylistWatcher()
        {
            _random = new Random();
        }

        public RandomSongPlaylistWatcher(int seed)
        {
            _random = new Random(seed);
        }

        public void AttachToPlaylist(Playlist playlist, ILibraryRepository library)
        {
            

            _playlist = playlist;
            _library = library;

            if (_playlist.Count == 0)
            {
                Console.WriteLine("Attaching");
                _playlist.AddRange(_library.GetAllMusic().Shuffle(_random).Take(PLAYLIST_SIZE));
            }
        }

        public void SongStarting(MusicInfo song)
        {
        }

        public void SongEnding(MusicInfo song)
        {
            if (_playlist.RemainingSongs > PLAYLIST_SIZE)
                return;

            //TODO: Don't get the whole library for each song change
            var allMusic = _library.GetAllMusic().Shuffle(_random).ToArray();
            for (var i = 0; i < allMusic.Length; i++)
            {
                if (!_playlist
                    .AsQueryable()
                    .Reverse()
                    .Take(allMusic.Length/2)
                    .Any(X => X.FullPath == allMusic[i].FullPath))
                {
                    _playlist.Add(allMusic[i]);
                    break;
                }
                    
            }
        }


        public void PlaylistChanged()
        {
        }
    }
}
