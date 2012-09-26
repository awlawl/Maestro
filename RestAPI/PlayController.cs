using System.Threading;
using System.Web.Http;
using MusicData;

namespace RestAPI
{
    public class PlayController : ApiController
    {
        public void Play()
        {
            (new Thread(new ThreadStart(Player.Current.Play))).Start();
        }
        
    }
}
