using Microsoft.EntityFrameworkCore;
using PlayStudioHugoR.Models.DbPlayContext;
using PlayStudioHugoR.Models.Entities;
using PlayStudioHugoR.Repository.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace PlayStudioHugoR.Repository
{
    public class EmailSender : IEmailSender
    {
        private readonly ISendGridClient _sendGridClient;
        private readonly PlayStudioDbContext dbContext;

        public EmailSender(PlayStudioDbContext dbContext, ISendGridClient sendGridClient)
        {
            this.dbContext = dbContext;
            _sendGridClient = sendGridClient;
        }
        public async Task<string> SendResetEmail(string username)
        {
            if (!dbContext.Users.Any(x => x.email == username))
            {
                throw new Exception("Email " + username + " does not exist");
            }
            var prod = "https://playstudiosrios.azurewebsites.net/Home/NewPassword?user=" +username;
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("huguitorude@gmail.com", "Assesment User"),
                Subject = "You've requested a password reset"
            };
            msg.AddContent(MimeType.Text, "Hello, We have sent you this email in response to your request to reset your password.\n" +
                "To reset your password, please follow the link below: \n" +
                prod);
            msg.AddTo(new EmailAddress(username, "Example User"));
            try
            {
                var response = await _sendGridClient.SendEmailAsync(msg).ConfigureAwait(false);
                var status = response.StatusCode;
                if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    return "Success";
                }
                else
                {
                    return "Failed";
                }
            }
            catch (Exception ex)
            {
                return "Failed";
                throw;
            }
        }
    }
}
