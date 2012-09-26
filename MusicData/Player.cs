
namespace MusicData
{
    public class Player
    {
        private IAudioInteractor _audioInteractor = null;
        private Playlist _playlist = null;
        public bool _paused = false;

        public int MaxPlayCount { get; set; }
        public int PlayCount { get; set; }

        public Player(Playlist playlist, IAudioInteractor audioInteractor)
        {
            _audioInteractor = audioInteractor;
            _playlist = playlist;
        }
        
        public void Play()
        {
            while (ShouldPlayASong())
            {
                _audioInteractor.PlaySong(_playlist.GetNextSong());
                PlayCount++;
            }
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
            _audioInteractor.PauseSong();
            _paused = true;
        }

        public void Resume()
        {
            if (_paused)
            {
                _audioInteractor.ResumeSong();
                _paused = false;
            }
        }

        public void Stop()
        {
            _audioInteractor.StopSong();
        }
        
    }
}
