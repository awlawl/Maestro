using System.Threading;
using System.Web.Http;
using MusicData;

namespace RestAPI
{
    public class PlayController : ApiController
    {
        [HttpGet]
        public void Play()
        {
            (new Thread(new ThreadStart(Player.Current.Play))).Start();
        }
        
    }
}
