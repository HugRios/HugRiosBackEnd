using PlayStudioHugoR.Models.Entities;

namespace PlayStudioHugoR.Repository.Interfaces
{
    public interface ILoginRepository
    {
        string SaveDataUser(UsersModel usersModel);
        string Login(string email, string password);

        string CheckUserExists(string email);
        string ChangePass(string email, string password);
    }
}
