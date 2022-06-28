using Authentication.Api.Models;

namespace Authentication.Api.Interfaces
{
    public interface IUserService
    {
        UserModel CreateUser(UserModel userModel);
        UserModel GetUserByEmail(string email);
        List<UserModel> GetAllUsers();
        bool IsUserEmailAndPasswordExist(LoginModel loginModel);
    }
}