using System.Threading;
using System.Web.Http;
using MusicData;

namespace RestAPI
{
    public class BackController : ApiController
    {
        [HttpGet]
        public void Back()
        {
            (new Thread(new ThreadStart(Player.Current.Back))).Start();
        }
        
    }
}
