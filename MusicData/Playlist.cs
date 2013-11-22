using System.Collections.Generic;

namespace MusicData
{
    public class Playlist : List<MusicInfo>
    {
        public List<IPlaylistWatcher> PlaylistWatcher { get; set; }
        private int _lastPlayedPosition = -1;

        public Playlist()
        {
            PlaylistWatcher = new List<IPlaylistWatcher>();  
        }

        public Playlist(IPlaylistWatcher watcher)
        {
            PlaylistWatcher = new List<IPlaylistWatcher>() { watcher};
        }

        public Playlist(IPlaylistWatcher[] watchers)
        {
            PlaylistWatcher = new List<IPlaylistWatcher>(); 
            PlaylistWatcher.AddRange(watchers);
        }

        public void MoveToNextSong()
        {
            if (CurrentPosition + 1 < this.Count)
            {
                CurrentPosition++;
            }
        }

        public void MoveBackOneSong()
        {
            if (CurrentPosition - 1 >= 0)
            {
                CurrentPosition--;
            }
            else
                _lastPlayedPosition = -1;
        }

        public MusicInfo CurrentSong {
            get
            {
                if (this.Count == 0)
                    return null;
                if (CurrentPosition >= this.Count)
                    return this[this.Count - 1];
                else
                    return this[CurrentPosition];
            }
        }

        public MusicInfo PreviousSong
        {
            get
            {
                if (CurrentPosition > 0)
                    return this[CurrentPosition - 1];
                else
                    return null;
            }
        }

        public int CurrentPosition { get; set; }
        
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
            get { return this.Count - (CurrentPosition + 1); }
        }

        public void CurrentSongIsStarting()
        {
            _lastPlayedPosition = CurrentPosition;

            foreach (var playlistWatcher in PlaylistWatcher)
                playlistWatcher.SongStarting(CurrentSong);
        }

        public void CurrentSongIsEnding()
        {
            _lastPlayedPosition = CurrentPosition;
            foreach (var playlistWatcher in PlaylistWatcher)
                playlistWatcher.SongEnding(CurrentSong);
        }

        public void ClearPlaylist()
        {
            this.Clear();
            this.CurrentPosition = 0;
        }
    }
}
