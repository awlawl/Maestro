
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

            Stream data = null;
            string contentType = "";
            byte[] blankgif = new byte[] {
                0x47,0x49, 0x46, 0x38, 0x39, 0x61, 0x01, 0x00, 0x01, 0x00, 0x80, 0x00, 0x00, 0xff, 0xff, 0xff, 0x00, 0x00, 0x00, 0x21, 0xf9, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2c, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x02, 0x02, 0X44, 0x01, 0x00, 0X3B
            };
            
            if (art == null)
            {
                data = new MemoryStream(blankgif);
                contentType = "image/gif";
                
            }
            else
            {
                data = new MemoryStream(art.RawData);
                contentType = art.ContentType;
            }

            var resp = new HttpResponseMessage()
            {
                Content = new StreamContent(data),
            };
                       

            resp.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            return resp;
        }
               
    }
}
