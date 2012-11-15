namespace RealTimeMessaging
{
    public class PubnubMessage
    {
        public const string ACTION_PLAY = "Play";
        public const string ACTION_STOP = "Stop";
        public const string ACTION_NEXT = "Next";
        public const string ACTION_BACK = "Back";
        public const string ACTION_NOWPLAYING = "Now Playing";

        public string action { get; set; }
        public dynamic data { get; set; }
        
    }
}
