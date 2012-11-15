using MusicData;

namespace RealTimeMessaging
{
    public class MessagingPlaylistWatcher : IPlaylistWatcher
    {
        private IRealTimeMessaging _messaging = null;

        
        public void AssignMessaging( IRealTimeMessaging messaging)
        {
            _messaging = messaging;
        }

        public void AttachToPlaylist(Playlist playlist, ILibraryRepository library)
        {
         
        }

        public void PlaySong(MusicInfo song)
        {
            _messaging.SendMessage(new PubnubMessage()
                {
                    action = PubnubMessage.ACTION_NOWPLAYING,
                    data = song
                });   
        }
    }
}
