using System.Linq;
using System.Collections.Generic;
using System.IO;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using MusicData;

namespace MongoLibrary
{
    public class MongoLibraryRepository : ILibraryRepository
    {
        public IRealTimeMessaging Messaging { get; set; }

        public MongoLibraryRepository(IRealTimeMessaging messaging) {
            Messaging = messaging;
        }

        public void AddMusicToLibrary(MusicInfo[] songs)
        {

            if (songs.Length == 0)
                return;

            var collection = MongoHelper.Current.GetCollection<MusicInfo>("musicinfo");

            collection.InsertBatch(songs);

            if (Messaging != null)
                Messaging.SendSongsAdded(songs);
        }

        public void AddOrUpdateMusicInLibrary(MusicInfo song)
        {
            Log.Debug("Adding song to library: " + song.FullPath);
            var collection = MongoHelper.Current.GetCollection<MongoMusicInfo>("musicinfo");
            

            //find any existing
            var existing = collection.FindAll()
                .Where(X => X.FullPath == song.FullPath)
                .ToArray();

            //insert it again
            collection.Insert(song);

            if (existing.Length>0)
            {
                //delete the original ones

                foreach (var previousVersion in existing)
                {
                    Log.Debug("This song was already in the library, deleting the old one: " + previousVersion.IdValue);
                    collection.Remove(Query.EQ("_id", previousVersion.Id));
                }
            }

            if (Messaging != null)
                Messaging.SendSongsAdded(new MusicInfo[] { song });
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

            if (!Directory.Exists(directoryPath))
            {
                Log.Debug("Couldn't add directory to library: " + directoryPath);
                return;
            }
            else
            {
                Log.Debug("Adding directory to library :" + directoryPath);
            }

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

        public MusicInfo GetSongById(string id)
        {
            return MongoHelper.Current.GetCollection<MongoMusicInfo>("musicinfo")
               .FindOneByIdAs<MongoMusicInfo>(ObjectId.Parse(id));
        }
        
        public List<SavedPlaylist> GetAllSavedPlaylists()
        {
            return MongoHelper.Current.GetCollection<MongoSavedPlaylist>("savedplaylist")
                .FindAllAs<MongoSavedPlaylist>()
                .OrderBy(X => X.Name)
                .ToList<SavedPlaylist>();
        }

        public List<MusicInfo> GetAllSongsForSavedPlaylist(string name)
        {
            return MongoHelper.Current.GetCollection<MongoMusicInfo>("musicinfo")
                .FindAll()
                .Where(X=> X.SavedPlaylists.Any(Y => Y == name))
                .ToList<MusicInfo>();

        }

        public SavedPlaylist AddNewSavedPlaylist(string name)
        {
            var newOne = new MongoSavedPlaylist()
            {
                Name = name
            };

            MongoHelper.Current.GetCollection<MongoSavedPlaylist>("savedplaylist")
                .Insert(newOne);

            return newOne;
        }

        public void AddSongToSavedPlaylist(string savedPlaylistName, string songId)
        {
            var collection = MongoHelper.Current.GetCollection<MongoMusicInfo>("musicinfo");
            var song = collection
              .FindOneByIdAs<MongoMusicInfo>(ObjectId.Parse(songId));

            song.SavedPlaylists = song.SavedPlaylists.Union(new string[] { savedPlaylistName }).ToArray();

            collection.Save(song);

        }

        public void RemoveSongFromSavedPlaylist(string savedPlaylistName, string songId)
        {
            var collection = MongoHelper.Current.GetCollection<MongoMusicInfo>("musicinfo");
            var song = collection
              .FindOneByIdAs<MongoMusicInfo>(ObjectId.Parse(songId));

            song.SavedPlaylists = song.SavedPlaylists.Where(X => X != savedPlaylistName).ToArray();

            collection.Save(song);

        }

        public void RemoveSavedPlaylist(string savedPlaylistId)
        {
            var collection = MongoHelper.Current.GetCollection<MongoSavedPlaylist>("savedplaylist");
            collection.Remove(Query<MongoSavedPlaylist>.EQ(e => e.Id, ObjectId.Parse(savedPlaylistId)),WriteConcern.Acknowledged);
        }
    }
}
