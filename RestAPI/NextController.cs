using System.Threading;
using System.Web.Http;
using MusicData;

namespace RestAPI
{
    public class NextController : ApiController
    {
        [HttpGet]
        public void Next()
        {
            (new Thread(new ThreadStart(Player.Current.Next))).Start();
        }
        
    }
}
