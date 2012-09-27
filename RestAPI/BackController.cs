using System.Threading;
using System.Web.Http;
using MusicData;

namespace RestAPI
{
    public class BackController : ApiController
    {
        public void Back()
        {
            Player.Current.Back();
        }
        
    }
}
