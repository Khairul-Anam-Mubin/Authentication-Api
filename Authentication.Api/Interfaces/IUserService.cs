using Authentication.Api.Models;

namespace Authentication.Api.Interfaces
{
    public interface IUserService
    {
        UserModel CreateUser(UserModel userModel);
        UserModel UpdateUser(UserModel userModel);
        UserModel DeleteUser(string id);
        UserModel GetUser(string id);
        List<UserModel> GetAllUsers();
        UserModel GetUserByEmail(string email);
        bool IsUserEmailAndPasswordExist(LoginModel loginModel);
    }
}