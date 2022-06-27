using Authentication.Shared.Library.Models;
using Authentication.Business.Interfaces;
using Authentication.Shared.Library.Constants;

namespace Authentication.Business.Services
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
    }
}