namespace MusicData
{
    public class Player : IPlayer
    {
        private IAudioInteractor _audioInteractor = null;
        public Playlist Playlist {get;set;}
        public ILibraryRepository Library { get; set; }
        private bool _stopped = false;

        public int MaxPlayCount { get; set; }
        public int PlayCount { get; set; }
        private bool _isPlaying = false;

        private static Player _current;

        public static Player Current {
            get
            {
                return _current;
            }
        }

        public bool IsPaused { get; set; }

        public Player(Playlist playlist, IAudioInteractor audioInteractor, ILibraryRepository library)
        {
            _audioInteractor = audioInteractor;
            Playlist = playlist;
            _current = this;
            Library = library;
        }
        
        public void Play()
        {
            if (_isPlaying)
                return;
            
            _stopped = false;
            _isPlaying = true;

            Log.Debug("Play starting");

            if (ShouldPlayASong())
            {

                while (!_stopped)
                {
                    Playlist.CurrentSongIsStarting();
                    _audioInteractor.PlaySong(Playlist.CurrentSong.FullPath);
                    PlayCount++;
                    Playlist.CurrentSongIsEnding();

                    if (!ShouldPlayASong())
                        break;

                    if (!_stopped)
                        Playlist.MoveToNextSong();

                }
            }
            _isPlaying = false;
        }

        private bool ShouldPlayASong()
        {
            if (MaxPlayCount > 0)
                return Playlist.AreMoreSongsAvailable() && PlayCount < MaxPlayCount;
            else
                return Playlist.AreMoreSongsAvailable();
            
        }

        public void Pause()
        {
            Log.Debug("Pausing");
            _audioInteractor.PauseSong();
            IsPaused = true;
        }

        public void Resume()
        {
            Log.Debug("Resuming");
            if (IsPaused)
            {
                _audioInteractor.ResumeSong();
                IsPaused = false;
            }
        }

        public void Next()
        {
            Log.Debug("Next-ing.");
            _audioInteractor.StopSong();
            //Stop();
            /*Playlist.MoveToNextSong();
            Play();*/
        }

        public void Back()
        {
            Log.Debug("Back-ing");

            Playlist.MoveBackOneSong();
            Playlist.MoveBackOneSong();
            _audioInteractor.StopSong();
            

        }

        public void Stop()
        {
            Log.Debug("Stopping");
            _stopped = true;
            _audioInteractor.StopSong();
        }

        public void JumpToPlaylistIndex(int playlistIndex)
        {
            Log.Debug("Jumping to index in playlist " + playlistIndex);
            this.Playlist.CurrentPosition = playlistIndex-1;
            _audioInteractor.StopSong();
            
            //this.Play();
        }
    }
}
