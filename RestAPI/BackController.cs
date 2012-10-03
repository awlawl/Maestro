using System.Threading;
using System.Web.Http;
using MusicData;

namespace RestAPI
{
    public class BackController : ApiController
    {
        public void Back()
        {
            (new Thread(new ThreadStart(Player.Current.Back))).Start();
        }
        
    }
}
