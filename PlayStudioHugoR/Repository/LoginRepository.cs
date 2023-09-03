using PlayStudioHugoR.Models.DbPlayContext;
using PlayStudioHugoR.Models.Entities;
using PlayStudioHugoR.Repository.Interfaces;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace PlayStudioHugoR.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly PlayStudioDbContext dbContext;
        private readonly IConfiguration _configuration;

        public LoginRepository(PlayStudioDbContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            _configuration = configuration;
        }

        public string SaveDataUser(UsersModel usersModel)
        {
            try
            {
                if (dbContext.Users.Any(x => x.email == usersModel.email))
                {
                    throw new Exception("Email " + usersModel.email + " is already taken");
                }
                usersModel.password = BCrypt.Net.BCrypt.HashPassword(usersModel.password);
                dbContext.Add<UsersModel>(usersModel);
                dbContext.SaveChanges();
                return "inserted";
            }
            catch (Exception e)
            {
                return "Error: "+ e.Message.ToString();
                throw;
            }
        
        }

        public string Login(string email, string password)
        {
            try
            {
                UsersModel user = new UsersModel();
                    user = dbContext.Users.SingleOrDefault(x => x.email == email);
                bool passIsValid = false;
                if (user != null) { 
                    passIsValid = BCrypt.Net.BCrypt.Verify(password, user.password);
                }
                if (passIsValid)
                {
                    return "logged";
                }
                else
                {
                    throw new Exception("User name or password is incorrect");
                }

            }
            catch (Exception e)
            {
                return e.Message.ToString();
                throw;
            }
            
        }

        public string CheckUserExists(string email)
        {
            var user = dbContext.Users.SingleOrDefault(x => x.email == email);
            if (user != null)
            {
                return "exists";
            }
            else
            {
                return "not user";
            }
        }

        public string ChangePass(string email, string password)
        {
            UsersModel user = dbContext.Users.SingleOrDefault(x => x.email == email);
            if (user != null)
            {
                user.password = BCrypt.Net.BCrypt.HashPassword(password);
                dbContext.Entry(user).State = EntityState.Modified;
                dbContext.SaveChanges();
                return "success";
            }
            else
            {
                return "error";
            }
        }
    }
}
