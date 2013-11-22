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

        public dynamic PlaySavedPlaylist(string savedPlaylistName)
        {
            var library = Player.Current.Library;

            List<MusicInfo> songs = library.GetAllSongsForSavedPlaylist(savedPlaylistName);

            if (songs.Count == 0)
                return "";

            foreach (var song in songs)
                Player.Current.Playlist.Enqueue(song);

            Player.Current.JumpToPlaylistByPath(songs[0].FullPath);

            return JsonConvert.SerializeObject(songs[0]);
        }
    }
}
