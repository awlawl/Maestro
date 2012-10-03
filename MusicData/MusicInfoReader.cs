
using System.Collections.Generic;
using System.IO;

namespace MusicData
{
    public class MusicInfoReader
    {
        //TODO: make this configurable somehow
        private string[] _supportedExtentions = {"mp3"};
       
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
            var files = Directory.GetFiles(audioDirectory, "*." + _supportedExtentions[0]);

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
