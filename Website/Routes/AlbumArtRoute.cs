using MusicData;

namespace Website.Routes
{
    public class AlbumArtRoute
    {
        public readonly byte[] BLANKGIF = new byte[] {
            0x47,0x49, 0x46, 0x38, 0x39, 0x61, 0x01, 0x00, 0x01, 0x00, 0x80, 0x00, 0x00, 0xff, 0xff, 0xff, 0x00, 0x00, 0x00, 
            0x21, 0xf9, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2c, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x01, 0x00, 
            0x00, 0x02, 0x02, 0X44, 0x01, 0x00, 0X3B
        };

        public ImageResponse GetAlbumArt()
        {
            //var musicInfoReader = new MusicInfoReader();
            //AlbumArt art = null;

            //if (Player.Current.Playlist.CurrentSong!=null)
            //    art = musicInfoReader.GetAlbumArtForFile(Player.Current.Playlist.CurrentSong.FullPath);

            byte[] data = null;
            string contentType = "";
            
            //if (art == null)
            //{
                data = BLANKGIF;
                contentType = "image/gif";
            //}
            //else
            //{
            //    data = art.RawData;
            //    contentType = art.ContentType;
            //}

            return new ImageResponse(data, contentType);
        }

    }
}
