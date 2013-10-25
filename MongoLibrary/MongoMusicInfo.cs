using System;
using MongoDB.Bson;
using MusicData;

namespace MongoLibrary
{
    public class MongoMusicInfo : MusicInfo
    {
        public MongoMusicInfo()
        {
            CreatedDate = DateTime.Now;
        }

        public ObjectId Id { get; set; }
        public DateTime CreatedDate { get; set; } 

        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        public string IdValue { get { return this.Id.ToString(); } }

    }
}
