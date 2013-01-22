using System.Linq;
using System.Collections.Generic;
using MongoDB.Driver.Linq;
using MusicData;

namespace MongoLibrary
{
    public class MongoLibraryRepository : ILibraryRepository
    {
        public void AddMusicToLibrary(MusicInfo[] songs)
        {
            var collection = MongoHelper.Current.GetCollection<MusicInfo>("musicinfo");

            collection.InsertBatch(songs);
        }

        public List<MusicInfo> GetAllMusic()
        {
            return MongoHelper.Current.GetCollection<MongoMusicInfo>("musicinfo")
                .FindAll()
                .Select(X => (MusicInfo)X) //is there a better way?
                .ToList();
        }

        public void AddDirectoryToLibrary(string directoryPath)
        {
            var musicInfoReader = new MusicInfoReader();

            List<MusicInfo> allMusic = musicInfoReader.CrawlDirectory(directoryPath);

            this.AddMusicToLibrary(allMusic.ToArray());
        }

        public void ClearLibrary()
        {
            MongoHelper.Current.GetCollection<MusicInfo>("musicinfo").RemoveAll();
        }
    }
}
