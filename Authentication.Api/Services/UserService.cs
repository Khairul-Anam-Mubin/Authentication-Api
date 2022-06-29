using Authentication.Api.Constants;
using Authentication.Api.Interfaces;
using Authentication.Api.Models;

namespace Authentication.Api.Services
{
    public class UserService : IUserService
    {
        private List<UserModel> userList = new List<UserModel>();
        public UserModel CreateUser(UserModel userModel)
        {
            if (string.IsNullOrEmpty(userModel.Email)) throw new Exception("Invalid Request. Email needed.");
            if (GetUserByEmail(userModel.Email) != null) throw new Exception("User Email already exist");
            userModel.CreatedAt = DateTime.Now;
            userModel.IsDeleted = false;
            userModel.UserRole = UserRole.Visitor;
            try
            {
                userList.Add(userModel);
            }
            catch (Exception ex)
            {
                throw new Exception("Create User failed.");
            }
            return userModel;
        }
        public UserModel GetUserByEmail(string email)
        {
            foreach (var user in userList)
            {
                if (user.Email == email) return user;
            }
            return null;
        }
        public List<UserModel> GetAllUsers()
        {
            return userList;
        }
        public bool IsUserEmailAndPasswordExist(LoginModel loginModel)
        {
            UserModel user = GetUserByEmail(loginModel.Email);
            if (user == null) return false;
            return (user.Password == loginModel.Password);
        }
    }
}