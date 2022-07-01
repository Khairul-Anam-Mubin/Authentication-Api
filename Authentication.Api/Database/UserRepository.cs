using Authentication.Api.Models;
using MongoDB.Driver;

namespace Authentication.Api.Database
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<UserModel> _userCollection;

        public UserRepository(IDatabaseClient databaseClient)
        {
            _userCollection = databaseClient.GetDatabase().GetCollection<UserModel>("UserModel");
        }

        public void Insert(UserModel user)
        {
            _userCollection.InsertOne(user);
        }

        public List<UserModel> GetAllUser()
        {
            return _userCollection.Find(user => true).ToList();
        }

        public UserModel GetUserByEmail(string email)
        {
            return _userCollection.Find(user => user.Email == email).FirstOrDefault();
        }
    }
}
