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
            userModel.CreatedAt = DateTime.Now;
            userModel.IsDeleted = false;
            userModel.UserRole = UserRole.Visitor;
            userList.Add(userModel);
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