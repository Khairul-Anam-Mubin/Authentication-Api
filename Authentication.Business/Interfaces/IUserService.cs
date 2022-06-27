using Authentication.Shared.Library.Models;

namespace Authentication.Business.Interfaces
{
    public interface IUserService
    {
        UserModel CreateUser(UserModel userModel);
        UserModel GetUserByEmail(string email);
        List<UserModel> GetAllUsers();
    }
}