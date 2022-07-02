using Authentication.Api.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Authentication.Api.Database
{
    public interface IDatabaseClient
    {
        public IMongoDatabase GetDatabase();
        public IMongoCollection<BsonDocument> GetCollection<T>() where T: class, IRepositoryItem;
        public void InsertItem<T>(T item) where T : class, IRepositoryItem;
        public void UpdateItem<T>(T item) where T : class, IRepositoryItem;
        public void DeleteItem<T>(string id) where T : class, IRepositoryItem;
        public T GetItem<T>(string id) where T : class, IRepositoryItem;
        public List<T> GetAllItems<T>() where T: class, IRepositoryItem;
    }
}