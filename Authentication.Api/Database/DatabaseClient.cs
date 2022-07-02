using Authentication.Api.Constants;
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
            var client = new MongoClient(configuration[DatabaseSettings.ConnectionSting]);
            _database = client.GetDatabase(configuration[DatabaseSettings.DatabaseName]);
        }
        public IMongoDatabase GetDatabase()
        {
            return _database;
        }
        public IMongoCollection<BsonDocument> GetCollection<T>() where T : class, IRepositoryItem
        {
            return GetDatabase().GetCollection<BsonDocument>(typeof(T).Name);
        }
        public void InsertItem<T>(T item) where T : class, IRepositoryItem
        {
            var itemInBsonDocument = item.ToBsonDocument();
            var collection = GetCollection<T>();
            collection.InsertOne(itemInBsonDocument);
        }
        public void UpdateItem<T>(T item) where T : class, IRepositoryItem
        {
            var collection = GetCollection<T>();
            var filter = Builders<BsonDocument>.Filter.Eq("_id", item.Id);
            var itemInBsonDocument = item.ToBsonDocument();
            collection.ReplaceOne(filter, itemInBsonDocument);
        }
        public void DeleteItem<T>(string id) where T : class, IRepositoryItem
        {
            var collection = GetCollection<T>();
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            collection.DeleteOne(filter);
        }
        public T GetItem<T>(string id) where T : class, IRepositoryItem
        {
            var collection = GetCollection<T>();
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var bsonDocument = collection.Find(filter).FirstOrDefault();
            return BsonSerializer.Deserialize<T>(bsonDocument.ToJson());
        }
        public List<T> GetAllItems<T>() where T : class, IRepositoryItem
        {
            var collection = GetCollection<T>();
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