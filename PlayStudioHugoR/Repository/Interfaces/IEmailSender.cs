namespace PlayStudioHugoR.Repository.Interfaces
{
    public interface IEmailSender
    {
        Task<string> SendResetEmail(string username);
    }
}
