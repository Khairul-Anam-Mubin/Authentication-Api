using Authentication.Api.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Authentication.Api.Database
{
    public interface IDatabaseClient
    {
        public IMongoDatabase GetDatabase();
        public IMongoCollection<BsonDocument> GetCollection(string collectionName);
        public void Insert<T>(T item) where T : class, IRepositoryItem;
        public List<T> GetAllItems<T>() where T: class, IRepositoryItem;
    }
}
