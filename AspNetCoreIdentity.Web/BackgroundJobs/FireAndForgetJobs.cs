namespace AspNetCoreIdentity.Web.BackgroundJobs;

public class FireAndForgetJobs
{
    public static void EmailsendToUserJob(string toEmail, string message)
    {
        BackgroundJob.Enqueue<IEmailService>(x => x.SendNotificationEmailForSiteRule(toEmail, message));
    }
}
