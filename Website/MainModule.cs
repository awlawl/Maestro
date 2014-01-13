using System.IO;
using System.Runtime.Remoting.Messaging;
using MusicData;
using Website.Routes;
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
                return File.ReadAllText(websitePath + "old Maestro.html");
            };

            Get["/new"] = x =>
            {
                return File.ReadAllText(websitePath + "Maestro.html");
            };

            Get["/NowPlaying"] = x =>
            {
                var currentSongJson = (Nancy.Response)JsonConvert.SerializeObject(Player.Current.Playlist.CurrentSong);
                currentSongJson.ContentType = "application/json";
                return currentSongJson;
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

            Delete["/Playlist/{id}/{index}"] = x =>
            {
                string id = x.id.ToString();
                int index = int.Parse(x.index.ToString());
                (new PlaylistRoute()).RemoveFromPlaylist(id, index);
                var response = (Nancy.Response)"";
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

            Post["/EnqueueSavedPlaylistWithShuffle/{name}"] = x =>
            {
                string name = x.name.ToString();
                var response = (Nancy.Response)(new EnqueueSongRoute()).EnqueueSavedPlaylistWithShuffle(name);
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

            Put["/Play"] = x =>
            {
                var response = (Nancy.Response)(new ButtonControllsRoute()).PlayButton();
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

            Put["/Stop"] = x =>
            {
                var response = (Nancy.Response)(new ButtonControllsRoute()).StopButton();
                response.ContentType = "application/json";
                return response;
            };

            Put["/Pause"] = x =>
            {
                var response = (Nancy.Response)(new ButtonControllsRoute()).PauseButton();
                response.ContentType = "application/json";
                return response;
                return "";
            };

            Put["/Next"] = x =>
            {
                var response = (Nancy.Response)(new ButtonControllsRoute()).NextButton();
                response.ContentType = "application/json";
                return response;
                return "";
            };

            Put["/Back"] = x =>
            {
                var response = (Nancy.Response)(new ButtonControllsRoute()).BackButton();
                response.ContentType = "application/json";
                return response;
                return "";
            };




        }

    }
}
