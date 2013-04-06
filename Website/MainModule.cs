using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MusicData;
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

            Get["/AlbumArt"] = x =>
            {
                var musicInfoReader = new MusicInfoReader();
                AlbumArt art = musicInfoReader.GetAlbumArtForFile(Player.Current.Playlist.CurrentSong.FullPath);

                byte[] data = null;
                string contentType = "";
                byte[] blankgif = new byte[] {
                0x47,0x49, 0x46, 0x38, 0x39, 0x61, 0x01, 0x00, 0x01, 0x00, 0x80, 0x00, 0x00, 0xff, 0xff, 0xff, 0x00, 0x00, 0x00, 0x21, 0xf9, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2c, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x02, 0x02, 0X44, 0x01, 0x00, 0X3B
            };

                if (art == null)
                {
                    data = blankgif;
                    contentType = "image/gif";

                }
                else
                {
                    data = art.RawData;
                    contentType = art.ContentType;
                }

                return new ImageResponse(data, contentType);
            };

            
        }

        private Stream Wtf()
        {
            return null;
        }
    }
}
