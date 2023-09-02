using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlayStudioHugoR.Models;
using PlayStudioHugoR.Models.DbPlayContext;
using PlayStudioHugoR.Models.Entities;
using PlayStudioHugoR.Repository;
using PlayStudioHugoR.Repository.Interfaces;
using SendGrid;

namespace PlayStudioHugoR.Controllers
{
    [ApiController]
    public class LoginController : Controller
    {
        private readonly PlayStudioDbContext dbContext;
        private readonly IConfiguration _configuration;
        private readonly ISendGridClient _sendGridClient;

        public LoginController(PlayStudioDbContext dbContext, IConfiguration configuration,
            ISendGridClient sendGridClient) {
            this.dbContext = dbContext;
            _configuration = configuration;
            _sendGridClient = sendGridClient;

        }

        [HttpGet]
        [Route("User/GetInfo")]
        public IActionResult GetInfo()
        {
            return Ok("Ok");
        }

        [HttpPost]
        [Route("User/Insert")]
        public IActionResult InsertUser([FromBody] UsersModel usersModel)
        {
            try
            {
                ILoginRepository loginRepository = new LoginRepository(dbContext, _configuration);
                string answer = "";
                if (ModelState.IsValid)
                {
                    answer = loginRepository.SaveDataUser(usersModel);
                    if(answer != "inserted")
                    {
                        return BadRequest(answer);
                    }
                    else
                    {
                        return Ok(answer);
                    }
                }
                else
                {
                    return BadRequest("Please review the data");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message.ToString()}");
                throw;
            }
        }

        [HttpPost]
        [Route("User/Login")]
        public IActionResult InsertUser([FromBody] LoginModel login)
        {
            try
            {
                ILoginRepository loginRepository = new LoginRepository(dbContext, _configuration);
                string answer = "";
                if (ModelState.IsValid)
                {
                    answer = loginRepository.Login(login.Username,
                        login.Password);
                    if (answer != "logged")
                    {
                        return BadRequest(answer);
                    }
                    else
                    {
                        return Ok(answer);
                    }
                }
                else
                {
                    return BadRequest("Please review the data");
                }
               
            }
            catch (Exception e)
            {
                return StatusCode(500,$"Internal server error: {e.Message.ToString()}");
                throw;
            }
        }

        [HttpPost]
        [Route("User/ResetPass")]
        public async Task<IActionResult> ResetPass([FromBody] string username)
        {
            try
            {
                ILoginRepository loginRepository = new LoginRepository(dbContext, _configuration);
                string validUser = loginRepository.CheckUserExists(username);
                if (!string.IsNullOrEmpty(validUser))
                {
                    IEmailSender emailSender = new EmailSender(dbContext, _sendGridClient);
                    string answer = "";
                    if (username != null)
                    {
                        answer = await emailSender.SendResetEmail(username);
                    }
                    return Ok(answer);
                }
                else
                {
                    return BadRequest("User does not exists");
                }
                
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message.ToString()}");
                throw;
            }
        }


        [HttpPost]
        [Route("User/ChangePass")]
        public async Task<IActionResult> ChangePass([FromBody] UsersModel usersModel)
        {
            try
            {
                ILoginRepository loginRepository = new LoginRepository(dbContext, _configuration);
                string changeStatus = loginRepository.ChangePass(usersModel.email, usersModel.password);
                if (changeStatus.Equals("success"))
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("User does not exists");
                }

            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message.ToString()}");
                throw;
            }
        }
    }
}
