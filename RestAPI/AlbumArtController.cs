
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using MusicData;

namespace RestAPI
{
    public class AlbumArtController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var musicInfoReader = new MusicInfoReader();
            AlbumArt art = musicInfoReader.GetAlbumArtForFile(Player.Current.Playlist.CurrentSong.FullPath);
            MemoryStream memoryStream = new MemoryStream(art.RawData);

            var resp = new HttpResponseMessage()
            {
                Content = new StreamContent(memoryStream),
            };

            

            resp.Content.Headers.ContentType = new MediaTypeHeaderValue(art.ContentType);

            return resp;
        }
    }
}
