using System.Threading;
using System.Web.Http;
using MusicData;

namespace RestAPI
{
    public class StopController : ApiController
    {
        [HttpGet]
        public void Stop()
        {
            Player.Current.Stop();
        }
        
    }
}
