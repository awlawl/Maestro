using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MusicData;

namespace Website.Routes
{
    public class PlaySongRoute
    {
        public dynamic PlaySong(string id)
        {
            var library = Player.Current.Library;

            var song = library.GetSongById(id);

            Player.Current.JumpToPlaylistByPath(song.FullPath);

            return JsonConvert.SerializeObject(song);
        }
    }
}
