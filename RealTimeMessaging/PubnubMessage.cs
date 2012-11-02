namespace RealTimeMessaging
{
    public class PubnubMessage
    {
        public const string ACTION_PLAY = "Play";
        public const string ACTION_STOP = "Stop";
        public const string ACTION_NEXT = "Next";
        public const string ACTION_BACK = "Back";

        public string Action { get; set; }
    }
}
