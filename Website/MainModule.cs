using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MusicData;
using Nancy;
using Website.Routes;
using Nancy.Extensions;
using Nancy.IO;
using Nancy.Responses;
using Newtonsoft.Json;

namespace Website
{
    public class MainModule : Nancy.NancyModule
    {
        public MainModule()
        {
            string websitePath = System.Configuration.ConfigurationManager.AppSettings["WebsitePath"];

            Get["/"] = x =>
            {
                return File.ReadAllText(websitePath + "Maestro.html");
            };

            Get["/Scripts/Maestro.js"] = x =>
            {
                return File.ReadAllText(websitePath + "Scripts\\Maestro.js");
            };

            Get["/AlbumArt"] = x =>
            {
                return (new AlbumArtRoute()).GetAlbumArt();
            };

            Get["/Playlist"] = x =>
            {
                var response = (Nancy.Response)(new PlaylistRoute()).GetPlayList();
                response.ContentType = "application/json";
                return response;
            };

            Get["/Search/{term}"] = x =>
                {
                    string q = x.term.ToString();
                    var response = (Nancy.Response)(new SearchRoute()).SearchLibrary(q);
                    response.ContentType = "application/json";
                    return response;
                };

            Post["/Enqueue/{id}"] = x =>
                {
                    string id = x.id.ToString();
                    var response = (Nancy.Response)(new EnqueueSongRoute()).EnqueueSong(id);
                    response.ContentType = "application/json";
                    return response;
                };

            Post["/EnqueueSavedPlaylist/{name}"] = x =>
            {
                string name = x.name.ToString();
                var response = (Nancy.Response)(new EnqueueSongRoute()).EnqueueSavedPlaylist(name);
                response.ContentType = "application/json";
                return response;
            };

            Post["/Play/{id}"] = x =>
            {
                string id = x.id.ToString();
                var response = (Nancy.Response)(new PlaySongRoute()).PlaySong(id);
                response.ContentType = "application/json";
                return response;
            };

            Post["/PlaySavedPlaylist/{name}"] = x =>
            {
                string name = x.name.ToString();
                var response = (Nancy.Response)(new PlaySongRoute()).PlaySavedPlaylist(name);
                response.ContentType = "application/json";
                return response;
            };

            Post["/PlaySavedPlaylistWithShuffle/{name}"] = x =>
            {
                string name = x.name.ToString();
                var response = (Nancy.Response)(new PlaySongRoute()).PlaySavedPlaylistWithShuffle(name);
                response.ContentType = "application/json";
                return response;
            };

            Get["/SavedPlaylist"] = x =>
                {
                    var response = (Nancy.Response)(new SavedPlaylistRoute()).GetAllSavedPlaylists();
                    response.ContentType = "application/json";
                    return response;
                };

            Get["/SavedPlaylist/{name}"] = x =>
            {
                string name = x.name.ToString();
                var response = (Nancy.Response)(new SavedPlaylistRoute()).GetSongsForSavedPlaylist(name);
                response.ContentType = "application/json";
                return response;
            };

            Post["/SavedPlaylist/{name}"] = x =>
            {
                string name = x.name.ToString();
                var response = (Nancy.Response)(new SavedPlaylistRoute()).AddNewSavedPlaylist(name);
                response.ContentType = "application/json";
                return response;
            };

            Post["/SavedPlaylist/{name}/{id}"]  = x =>
            {
                string name = x.name.ToString();
                string id = x.id.ToString();
                var response = (Nancy.Response)(new SavedPlaylistRoute()).AddSongToSavedPlaylist(name, id);
                response.ContentType = "application/json";
                return response;
            };

            Delete["/SavedPlaylist/{name}/{id}"] = x =>
            {
                string name = x.name.ToString();
                string id = x.id.ToString();
                var response = (Nancy.Response)(new SavedPlaylistRoute()).RemoveSongFromSavedPlaylist(name, id);
                response.ContentType = "application/json";
                return response;
            };

            Get["/PubNubChannel"] = x =>
            {
                var pubNubInfo = new { Channel=System.Configuration.ConfigurationManager.AppSettings["PubNubChannel"]};
                var response = (Nancy.Response)JsonConvert.SerializeObject(pubNubInfo);
                response.ContentType = "application/json";
                return response;
            };


        }

    }
}
