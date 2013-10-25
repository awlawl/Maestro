using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MusicData;

namespace Website.Routes
{
    public class EnqueueSongRoute
    {
        public dynamic EnqueueSong(string id)
        {
            var library = Player.Current.Library;

            var song = library.GetSongById(id);

            Player.Current.Playlist.Enqueue(song);

            return JsonConvert.SerializeObject(song);
        }
    }
}
