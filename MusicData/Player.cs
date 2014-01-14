using System.Linq;

namespace MusicData
{
    public class Player : IPlayer
    {
        public IAudioInteractor AudioInteractor { get; set; }
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
            AudioInteractor = audioInteractor;
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
                    AudioInteractor.PlaySong(Playlist.CurrentSong.FullPath);
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
            AudioInteractor.PauseSong();
            IsPaused = true;
        }

        public void Resume()
        {
            Log.Debug("Resuming");
            if (IsPaused)
            {
                AudioInteractor.ResumeSong();
                IsPaused = false;
            }
        }

        public void Next()
        {
            Log.Debug("Next-ing.");
            AudioInteractor.StopSong();
            //Stop();
            /*Playlist.MoveToNextSong();
            Play();*/
        }

        public void Back()
        {
            Log.Debug("Back-ing");

            Playlist.MoveBackOneSong();
            Playlist.MoveBackOneSong();
            AudioInteractor.StopSong();
            

        }

        public void Stop()
        {
            Log.Debug("Stopping");
            _stopped = true;
            AudioInteractor.StopSong();
        }

        public void JumpToPlaylistIndex(int playlistIndex)
        {
            Log.Debug("Jumping to index in playlist " + playlistIndex);
            this.Playlist.CurrentPosition = playlistIndex-1;
            AudioInteractor.StopSong();
            
            //this.Play();
        }

        public void JumpToPlaylistByPath(string path)
        {
            Log.Debug("Jumping to song " + path + " in playlist.");

            var found = this.Playlist.Where(X => X.FullPath == path).LastOrDefault();
            if (found == null)
                return;

            var playlistIndex = this.Playlist.LastIndexOf(found);
            //var playlistIndex = this.Playlist.IndexOf(found);

            this.Playlist.CurrentPosition = playlistIndex - 1;

            //if (_stopped)
            //    this.Play();

            AudioInteractor.StopSong();

            
        }
    }
}
