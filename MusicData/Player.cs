

namespace MusicData
{
    public class Player
    {
        private IAudioInteractor _audioInteractor = null;
        private Playlist _playlist = null;
        private bool _paused = false;
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

        public Player(Playlist playlist, IAudioInteractor audioInteractor)
        {
            _audioInteractor = audioInteractor;
            _playlist = playlist;
            _current = this;
        }
        
        public void Play()
        {
            if (_isPlaying)
                return;
            
            _stopped = false;
            _isPlaying = true;

            Log.Debug("Play starting");

            while (ShouldPlayASong() && !_stopped)
            {
                _audioInteractor.PlaySong(_playlist.GetNextSong());
                PlayCount++;
            }
            _isPlaying = false;
        }

        private bool ShouldPlayASong()
        {
            if (MaxPlayCount > 0)
                return _playlist.AreSongsAvailable() && PlayCount < MaxPlayCount;
            else
                return _playlist.AreSongsAvailable();
            
        }

        public void Pause()
        {
            Log.Debug("Pausing");
            _audioInteractor.PauseSong();
            _paused = true;
        }

        public void Resume()
        {
            Log.Debug("Resuming");
            if (_paused)
            {
                _audioInteractor.ResumeSong();
                _paused = false;
            }
        }

        public void Next()
        {
            Log.Debug("Next-ing.");
            Stop();
            Play();
        }

        public void Back()
        {
            Log.Debug("Back-ing");
            Stop();
            int beforeSize = _playlist.Count;
            _playlist.Enqueue(_playlist.GetLastSong());

            Play();
        }

        public void Stop()
        {
            Log.Debug("Stopping");
            _stopped = true;
            _audioInteractor.StopSong();
        }
        
    }
}
