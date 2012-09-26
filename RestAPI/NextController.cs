using System.Threading;
using System.Web.Http;
using MusicData;

namespace RestAPI
{
    public class NextController : ApiController
    {
        public void Next()
        {
            (new StopController()).Stop();
            //TODO: this is a giant hack, I think I need some way to block until it stops
            Thread.Sleep(100);
            (new PlayController()).Play();
        }
        
    }
}
