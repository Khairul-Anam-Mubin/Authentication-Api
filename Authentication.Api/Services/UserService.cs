using Authentication.Api.Constants;
using Authentication.Api.Database;
using Authentication.Api.Interfaces;
using Authentication.Api.Models;

namespace Authentication.Api.Services
{
    public class UserService : IUserService
    {
        private IUserRepository UserRepository { get; set; }
        private IDatabaseClient DatabaseClient { get; set; }
        public UserService(IUserRepository userRepository, IDatabaseClient databaseClient)
        {
            UserRepository = userRepository;
            DatabaseClient = databaseClient;
        }
        public UserModel CreateUser(UserModel userModel)
        {
            if (string.IsNullOrEmpty(userModel.Email)) throw new Exception("Invalid Request. Email needed.");
            if (GetUserByEmail(userModel.Email) != null) throw new Exception("User Email already exist");
            userModel.CreatedAt = DateTime.Now;
            userModel.IsDeleted = false;
            userModel.UserRole = UserRole.Visitor;
            try
            {
                DatabaseClient.Insert(userModel);
            }
            catch (Exception ex)
            {
                throw new Exception("Create User failed.");
            }
            return userModel;
        }
        public UserModel GetUserByEmail(string email)
        { 
            return UserRepository.GetUserByEmail(email);
        }
        public List<UserModel> GetAllUsers()
        {
            return DatabaseClient.GetAllItems<UserModel>();
        }
        public bool IsUserEmailAndPasswordExist(LoginModel loginModel)
        {
            UserModel user = GetUserByEmail(loginModel.Email);
            if (user == null) return false;
            return (user.Password == loginModel.Password);
        }
    }
}