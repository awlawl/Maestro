using System.Linq;
using MusicData;
using Newtonsoft.Json;

namespace Website.Routes
{
    public class PlaylistRoute
    {
        public const int HISTORY_TO_SHOW = 3;
        public const int FUTURE_TO_SHOW = 10;

        public string GetPlayList()
        {
            return JsonConvert.SerializeObject(GetPlaylistData());
        }

        public dynamic GetPlaylistData()
        {
            var playlist = Player.Current.Playlist;
            var historyToShow = 0;
            if (playlist.CurrentPosition >= HISTORY_TO_SHOW)
                historyToShow = playlist.CurrentPosition - HISTORY_TO_SHOW;

            var shortPlaylist = playlist
                .Select(X => new {
                    Song = X,
                    PlaylistIndex = playlist.IndexOf(X),
                })
                .Skip(historyToShow)
                .Take(FUTURE_TO_SHOW)
                .ToList();

            var response = new
            {
                Playlist = shortPlaylist,
                CurrentSongIndex = playlist.CurrentPosition
            };

            return response;
        }

        
    }
}
