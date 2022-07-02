using Authentication.Api.Constants;
using Authentication.Api.Database;
using Authentication.Api.Interfaces;
using Authentication.Api.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Authentication.Api.Services
{
    public class UserService : IUserService
    {
        private IDatabaseClient DatabaseClient { get; set; }
        public UserService(IDatabaseClient databaseClient)
        {
            DatabaseClient = databaseClient;
        }
        public UserModel CreateUser(UserModel userModel)
        {
            if (string.IsNullOrEmpty(userModel.Email)) throw new Exception("Invalid Request. Email needed.");
            //if (GetUserByEmail(userModel.Email) != null) throw new Exception("User Email already exist");
            userModel.CreatedAt = DateTime.Now;
            userModel.IsDeleted = false;
            userModel.UserRole = UserRole.Visitor;
            userModel.Id = Guid.NewGuid().ToString();
            try
            {
                DatabaseClient.InsertItem(userModel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return userModel;
        }
        public UserModel UpdateUser(UserModel userModel)
        {
            try
            {
                DatabaseClient.UpdateItem<UserModel>(userModel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return userModel;
        }
        public UserModel DeleteUser(string id)
        {
            UserModel user;
            try
            {
                user = GetUser(id);
                DatabaseClient.DeleteItem<UserModel>(id);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return user;
        }
        public UserModel GetUser(string id)
        {
            UserModel user;
            try
            {
                user = DatabaseClient.GetItem<UserModel>(id);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return user;
        }
        public List<UserModel> GetAllUsers()
        {
            return DatabaseClient.GetAllItems<UserModel>();
        }
        public UserModel GetUserByEmail(string email)
        {
            var collection = DatabaseClient.GetCollection<UserModel>();
            var filter = new BsonDocument("Email", email);
            var bsonVal= collection.Find(filter).FirstOrDefault();
            return BsonSerializer.Deserialize<UserModel>(bsonVal);
        }
        public bool IsUserEmailAndPasswordExist(LoginModel loginModel)
        {
            UserModel user = GetUserByEmail(loginModel.Email);
            if (user == null) return false;
            return (user.Password == loginModel.Password);
        }
    }
}