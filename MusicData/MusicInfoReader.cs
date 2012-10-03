
using System.Collections.Generic;
using System.IO;

namespace MusicData
{
    public class MusicInfoReader
    {
       
        public MusicInfo GetInfoForFile(string fullPath)
        {
             var result = new MusicInfo();

            TagLib.File file = TagLib.File.Create(fullPath);
            result.Artist = file.Tag.FirstAlbumArtist;
            result.Album = file.Tag.Album;
            result.Title = file.Tag.Title;
            result.FullPath = fullPath;

            return result;
        }




        public List<MusicInfo> CrawlDirectory(string audioDirectory)
        {
            var result = new List<MusicInfo>();

            var musicInfoReader = new MusicInfoReader();
            var files = Directory.GetFiles(audioDirectory);

            foreach (var file in files)
            {
                result.Add(musicInfoReader.GetInfoForFile(file));
            }

            return result;
        }
    }
}
