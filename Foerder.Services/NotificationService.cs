using Foerder.Domain;

namespace Foerder.Services
{
    public class NotificationService
    {
        private IEmailer Emailer { get; }
        private ISmsGateway SmsGateway { get; }

        public NotificationService(IEmailer emailer, ISmsGateway smsGateway)
        {
            Emailer = emailer;
            SmsGateway = smsGateway;
        }

        public void NotifyBewilligung(Foerderantrag antrag)
        {
            Emailer.SendMail();
        }
    }
}