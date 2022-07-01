using Authentication.Api.Models;

namespace Authentication.Api.Database
{
    public interface IUserRepository
    {
        public void Insert(UserModel user);
        public List<UserModel> GetAllUser();
        public UserModel GetUserByEmail(string email);
    }
}