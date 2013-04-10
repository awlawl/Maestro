using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MusicData;
using Website.Routes;
using Nancy.Extensions;
using Nancy.IO;
using Nancy.Responses;

namespace Website
{
    public class MainModule : Nancy.NancyModule
    {
        public MainModule()
        {
            Get["/"] = x =>
            {
                return File.ReadAllText(@"..\..\..\Website\Pages\Maestro.html");
            };

            Get["/Scripts/Maestro.js"] = x =>
            {
                return File.ReadAllText(@"..\..\..\Website\Pages\Scripts\Maestro.js");
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

               



            
        }

    }
}
