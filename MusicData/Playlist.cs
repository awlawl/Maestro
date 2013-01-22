
using System.Collections;
using System.Collections.Generic;

namespace MusicData
{
    public class Playlist : List<MusicInfo>
    {
        public IPlaylistWatcher[] PlaylistWatcher { get; set; }
        private int _currentPosition = 0;
        private int _lastPlayedPosition = -1;

        public Playlist(IPlaylistWatcher watcher)
        {
            PlaylistWatcher = new IPlaylistWatcher[] { watcher};
        }

        public Playlist(IPlaylistWatcher[] watcher)
        {
            PlaylistWatcher = watcher;
        }

        public void MoveToNextSong()
        {
            if (_currentPosition + 1 < this.Count)
            {
                _currentPosition++;
            }
        }

        public void MoveBackOneSong()
        {
            if (_currentPosition - 1 >= 0)
            {
                _currentPosition--;
            }
            else
                _lastPlayedPosition = -1;
        }

        public MusicInfo CurrentSong {
            get { return this[_currentPosition]; }
        }

        public MusicInfo PreviousSong
        {
            get
            {
                if (_currentPosition > 0)
                    return this[_currentPosition - 1];
                else
                    return null;
            }
        }
        
        public bool AreMoreSongsAvailable()
        {
            if (_lastPlayedPosition == -1)
                return this.Count > 0;
            else
                return this.RemainingSongs > 0;
        }
        
        public void Enqueue(MusicInfo song)
        {
            this.Add(song);
        }

        public int RemainingSongs
        {
            get { return this.Count - (_currentPosition + 1); }
        }

        public void CurrentSongIsStarting()
        {
            _lastPlayedPosition = _currentPosition;

            foreach (var playlistWatcher in PlaylistWatcher)
                playlistWatcher.SongStarting(CurrentSong);
        }

        public void CurrentSongIsEnding()
        {
            _lastPlayedPosition = _currentPosition;
            foreach (var playlistWatcher in PlaylistWatcher)
                playlistWatcher.SongEnding(CurrentSong);
        }
    }
}
