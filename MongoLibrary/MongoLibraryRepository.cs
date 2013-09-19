using System.Linq;
using System.Collections.Generic;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
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


        public List<MusicInfo> SearchLibrary(string term)
        {
            var query = Query.Or(
                    Query.Where(BsonJavaScript.Create(string.Format("this.Title.toLowerCase().indexOf('{0}') >= 0", term.ToLower()))),
                    Query.Where(BsonJavaScript.Create(string.Format("this.Album.toLowerCase().indexOf('{0}') >= 0", term.ToLower()))),
                    Query.Where(BsonJavaScript.Create(string.Format("this.Artist.toLowerCase().indexOf('{0}') >= 0", term.ToLower())))
                );
            return MongoHelper.Current.GetCollection<MongoMusicInfo>("musicinfo")
                .FindAs<MongoMusicInfo>(query)
                .Take(30)
                .ToList<MusicInfo>();
        }
    }
}
