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

        public dynamic EnqueueSavedPlaylist(string savedPlaylistName)
        {
            var library = Player.Current.Library;

            List<MusicInfo> songs = library.GetAllSongsForSavedPlaylist(savedPlaylistName);

            foreach (var song in songs)
                Player.Current.Playlist.Enqueue(song);

            return "";
        }

        public dynamic EnqueueSavedPlaylistWithShuffle(string savedPlaylistName)
        {
            var library = Player.Current.Library;

            List<MusicInfo> songs = library.GetAllSongsForSavedPlaylist(savedPlaylistName);

            var shuffledSongs = songs.Shuffle();

            foreach (var song in shuffledSongs)
                Player.Current.Playlist.Enqueue(song);

            return "";
        }
    }
}
