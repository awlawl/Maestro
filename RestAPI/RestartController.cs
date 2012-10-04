using System.Threading;
using System.Web.Http;
using MusicData;

namespace RestAPI
{
    public class RestartController : ApiController
    {

        [HttpPost]
        public void Restart()
        {
            Player.Current.Stop();
            var library = new MemoryLibraryRepository();
            Player.Current.Playlist.PlaylistWatcher.AttachToPlaylist(Player.Current.Playlist, library);

            (new Thread(new ThreadStart(Player.Current.Play))).Start();
        }
        
    }
}
