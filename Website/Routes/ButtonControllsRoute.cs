using System.Threading;
using MusicData;

namespace Website.Routes
{
    public class ButtonControllsRoute
    {
        public dynamic PlayButton()
        {
            Player player = Player.Current;
            if (player.IsPaused)
            {
                player.Resume();
            }
            else
            {
                (new Thread(player.Play)).Start();
            }

            return "";
        }

        public dynamic StopButton()
        {
            Player.Current.Stop();
            return "";
        }

        public dynamic PauseButton()
        {
            Player.Current.Pause();

            return "";
        }

        public dynamic NextButton()
        {
            Player.Current.Next();
            return "";
        }

        public dynamic BackButton()
        {
            Player.Current.Back();
            return "";
        }

    }
}