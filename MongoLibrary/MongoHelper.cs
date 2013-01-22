using System;
using System.Configuration;
using MongoDB.Driver;

namespace MongoLibrary
{
    public class MongoHelper
    {
        private static MongoDatabase _currentDatabase = null;

        public static MongoDatabase Current
        {
            get
            {
                if (_currentDatabase == null)
                    StartConnection();

                return _currentDatabase;
            }
        }

        private static void StartConnection()
        {
            var connectionString = ConfigurationManager.AppSettings["MongoConnection"];
            var connection = new MongoClient(connectionString);
            var server = connection.GetServer();
            var databaseName = (new Uri(connectionString)).PathAndQuery.Replace("/","");
            _currentDatabase = server.GetDatabase(databaseName);
        }
    }
}
