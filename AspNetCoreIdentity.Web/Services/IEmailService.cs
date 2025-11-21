namespace AspNetCoreIdentity.Web.Services
{
    public interface IEmailService
    {
        Task SendResetPasswordEmail(string resetPasswordEmailLink, string toEmail);

        Task SendNotificationEmailForSiteRule(string toEmail, string message);
    }
}
