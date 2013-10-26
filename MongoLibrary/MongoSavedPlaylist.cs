using System;
using MongoDB.Bson;
using MusicData;

namespace MongoLibrary
{
    class MongoSavedPlaylist : SavedPlaylist
    {
        public MongoSavedPlaylist()
        {
            CreatedDate = DateTime.Now;
        }

        public ObjectId Id { get; set; }
        public DateTime CreatedDate { get; set; } 

        [MongoDB.Bson.Serialization.Attributes.BsonIgnore]
        public string IdValue { get { return this.Id.ToString(); } }
    }
}
