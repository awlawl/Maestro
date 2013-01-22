namespace RealTimeMessaging
{
    public interface IRealTimeMessaging
    {
        void SendMessage(PubnubMessage message);

        void SendNowPlaying(MusicData.MusicInfo song);
        void SendPingReply();
    }
}