namespace MusicData
{
    public interface IRealTimeMessaging
    {
        void SendNowPlaying(MusicInfo song);
        void SendPingReply();
        void SendSongsAdded(MusicInfo[] song);
    }
}