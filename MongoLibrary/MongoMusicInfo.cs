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

    }
}
