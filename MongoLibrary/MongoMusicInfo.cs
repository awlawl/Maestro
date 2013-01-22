using MongoDB.Bson;
using MusicData;

namespace MongoLibrary
{
    public class MongoMusicInfo:MusicInfo
    {
        public ObjectId Id { get; set; } 
    }
}
