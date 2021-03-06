﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MusicData;

namespace Website.Routes
{
    public class SavedPlaylistRoute
    {
        public dynamic GetAllSavedPlaylists()
        {
            var library = Player.Current.Library;

            var playlists = library.GetAllSavedPlaylists();

            return JsonConvert.SerializeObject(playlists);
        }

        public dynamic GetSongsForSavedPlaylist(string savedPlaylistName)
        {
            var library = Player.Current.Library;

            var playlists = library.GetAllSongsForSavedPlaylist(savedPlaylistName);

            return JsonConvert.SerializeObject(playlists);
        }

        public dynamic AddNewSavedPlaylist(string savedPlaylistName)
        {
            var library = Player.Current.Library;

            var newSavedPlaylist = library.AddNewSavedPlaylist(savedPlaylistName);

            return JsonConvert.SerializeObject(newSavedPlaylist);
        }

        public dynamic AddSongToSavedPlaylist(string savedPlaylistName, string songId)
        {
            var library = Player.Current.Library;

            library.AddSongToSavedPlaylist(savedPlaylistName, songId);

            return "";
        }

        public dynamic RemoveSongFromSavedPlaylist(string savedPlaylistName, string songId)
        {
            var library = Player.Current.Library;

            library.RemoveSongFromSavedPlaylist(savedPlaylistName, songId);

            return "";
        }

        public dynamic RemoveSavedPlaylist(string savedPlaylistName)
        {
            var library = Player.Current.Library;
            library.RemoveSavedPlaylist(savedPlaylistName);

            return "";
        }
            

            
    }
}
