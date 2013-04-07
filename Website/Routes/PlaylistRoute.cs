using MusicData;
using Newtonsoft.Json;

namespace Website.Routes
{
    public class PlaylistRoute
    {
        public string GetPlayList()
        {
            var response = new { 
                  Player.Current.Playlist,
                  Player.Current.Playlist.CurrentPosition
            };
            return JsonConvert.SerializeObject(response);
        }
    }
}
