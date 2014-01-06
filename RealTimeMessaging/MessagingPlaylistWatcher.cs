using MusicData;

namespace RealTimeMessaging
{
    public class MessagingPlaylistWatcher : IPlaylistWatcher
    {
        private IRealTimeMessaging _messaging = null;
        private Playlist _playlist = null;
        
        
        public void AssignMessaging( IRealTimeMessaging messaging)
        {
            _messaging = messaging;
        }

        public void AttachToPlaylist(Playlist playlist, ILibraryRepository library)
        {
            _playlist = playlist;
        }

        public void SongStarting(MusicInfo song)
        {
            _messaging.SendNowPlaying(song);
        }

        public void SongEnding(MusicInfo song)
        {
            
        }


        public void PlaylistChanged()
        {
            _messaging.SendNowPlaying(_playlist.CurrentSong);
        }
    }
}
