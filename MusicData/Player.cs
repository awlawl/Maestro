﻿namespace MusicData
{
    public class Player : IPlayer
    {
        private IAudioInteractor _audioInteractor = null;
        public Playlist Playlist {get;set;}
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

        public Player(Playlist playlist, IAudioInteractor audioInteractor)
        {
            _audioInteractor = audioInteractor;
            Playlist = playlist;
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
                _audioInteractor.PlaySong(Playlist.GetNextSong().FullPath);
                PlayCount++;
            }
            _isPlaying = false;
        }

        private bool ShouldPlayASong()
        {
            if (MaxPlayCount > 0)
                return Playlist.AreSongsAvailable() && PlayCount < MaxPlayCount;
            else
                return Playlist.AreSongsAvailable();
            
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
            Stop();
            Play();
        }

        public void Back()
        {
            Log.Debug("Back-ing");
            
            MusicInfo lastSong = Playlist.GetLastSong();
            var copy = new MusicInfo[Playlist.Count];
            Playlist.CopyTo(copy,0);
            Playlist.Clear();
            Playlist.Enqueue(lastSong);

            foreach (var song in copy)
                Playlist.Enqueue(song);

            Stop();
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
