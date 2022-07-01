using System.Security.Cryptography.Xml;
using Authentication.Api.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Authentication.Api.Database
{
    public class DatabaseClient : IDatabaseClient
    {
        private readonly IMongoDatabase _database;
        public DatabaseClient(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["DatabaseSettings:ConnectionString"]);
            _database = client.GetDatabase(configuration["DatabaseSettings:DatabaseName"]);
        }
        public IMongoDatabase GetDatabase()
        {
            return _database;
        }
        public IMongoCollection<BsonDocument> GetCollection(string collectionName)
        {
            return GetDatabase().GetCollection<BsonDocument>(collectionName);
        }
        public void Insert<T>(T item) where T : class, IRepositoryItem
        {
            var itemInBsonDocument = item.ToBsonDocument();
            var collection = GetDatabase().GetCollection<BsonDocument>(typeof(T).Name);
            collection.InsertOne(itemInBsonDocument);
        }
        public List<T> GetAllItems<T>() where T : class, IRepositoryItem
        {
            var collection = GetDatabase().GetCollection<BsonDocument>(typeof(T).Name);
            var bsonDocuments = collection.Find(data => true).ToList();
            var result = new List<T>();
            foreach (var bsonDocument in bsonDocuments)
            {
                result.Add(BsonSerializer.Deserialize<T>(bsonDocument.ToJson()));
            }
            return result;
        }
    }
}