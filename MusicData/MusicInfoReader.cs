
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MusicData
{
    public class MusicInfoReader
    {
        //TODO: make this configurable somehow
        private string[] _supportedExtentions = {".mp3",".m4a"};
       
        public MusicInfo GetInfoForFile(string fullPath)
        {
             var result = new MusicInfo();

            TagLib.File file = TagLib.File.Create(fullPath);
            result.Artist = (file.Tag.FirstAlbumArtist ?? file.Tag.FirstPerformer) ?? "";
            result.Album = file.Tag.Album ?? "";
            result.Title = file.Tag.Title ?? "";
            result.FullPath = fullPath;
            result.TrackNumber = file.Tag.Track;

            if (string.IsNullOrEmpty(result.Title))
            {
                result.Title = Path.GetFileNameWithoutExtension(fullPath);
            }

            result.SavedPlaylists = new string[0];
            
            return result;
        }

        public AlbumArt GetAlbumArtForFile(string fullPath)
        {
            TagLib.File file = TagLib.File.Create(fullPath);

            var pic = file.Tag.Pictures.FirstOrDefault();

            if (pic == null)
                return null;
            else
            {
                return new AlbumArt()
                    {
                        ContentType = pic.MimeType,
                        RawData = pic.Data.Data
                    };
            }
        }




        public List<MusicInfo> CrawlDirectory(string audioDirectory)
        {
            var result = new List<MusicInfo>();

            var musicInfoReader = new MusicInfoReader();
            var files = Directory.GetFiles(audioDirectory).Where(X =>  _supportedExtentions.Contains(Path.GetExtension(X))).ToArray();


            foreach (var file in files)
            {
                result.Add(musicInfoReader.GetInfoForFile(file));
            }

            //look for subdirectories, recursively get files
            var directories = Directory.GetDirectories(audioDirectory);

            foreach (var directory in directories)
            {
                result.AddRange(CrawlDirectory(directory));
            }

            return result;
        }
    }
}
