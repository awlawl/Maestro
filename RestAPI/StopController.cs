using System.Threading;
using System.Web.Http;
using MusicData;

namespace RestAPI
{
    public class StopController : ApiController
    {
        public void Stop()
        {
            Player.Current.Stop();
        }
        
    }
}
