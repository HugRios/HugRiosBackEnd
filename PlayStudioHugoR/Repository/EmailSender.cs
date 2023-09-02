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
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("huguitorude@gmail.com", "Example User"),
                Subject = "Sending with Twilio SendGrid is Fun"
            };
            msg.AddContent(MimeType.Text, "and easy to do anywhere, even with C#");
            msg.AddTo(new EmailAddress("hugo.rios.cr@gmail.com", "Example User"));
            msg.TemplateId = "d-3dfd08f496114e5aa16f6490581180a1";
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
