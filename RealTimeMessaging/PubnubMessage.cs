namespace RealTimeMessaging
{
    public class PubnubMessage
    {
        public const string ACTION_PLAY = "Play";
        public const string ACTION_STOP = "Stop";
        public const string ACTION_NEXT = "Next";
        public const string ACTION_BACK = "Back";
        public const string ACTION_PAUSE = "Pause";
        public const string ACTION_NOWPLAYING = "Now Playing";
        public const string ACTION_PING = "Ping";
        public const string ACTION_PING_REPLY = "Ping Reply";
        public const string ACTION_PLAY_FROM_PLAYLIST = "PlayFromPlaylist";

        public string action { get; set; }
        public dynamic data { get; set; }
        
    }
}
