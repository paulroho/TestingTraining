using Foerder.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Foerder.Services.Tests
{
    [TestClass]
    public class NotificationServiceTests
    {
        [TestMethod]
        public void NotifyBewilligung_OnPrimaryNotificationTypeEmail_SendsJustAnEmail()
        {
            var smsGatewayMock = new Mock<ISmsGateway>();
            var emailerMock = new Mock<IEmailer>();
            var notificationService = new NotificationService(emailerMock.Object, smsGatewayMock.Object);

            var antrag = new Foerderantrag
            {
                PrimaryNotificationType = NotificationType.Email
            };

            // Act
            notificationService.NotifyBewilligung(antrag);

            emailerMock.Verify(emailer => emailer.SendMail(), Times.Once);
            smsGatewayMock.Verify(gateway => gateway.SendSms(), Times.Never);
        }
    }
}
