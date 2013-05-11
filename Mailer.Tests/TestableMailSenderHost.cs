using System;
using System.Threading.Tasks;
using Moq;

namespace Mailer
{
    public class TestableMailSenderHost : MailSenderHost
    {
        public readonly Mock<IMailQueue> Queue;
        public readonly Mock<IMailConfiguration> Configuration;
        public readonly Mock<IMailSender> Sender;
        public readonly Mock<ILog> Logger;

        TestableMailSenderHost(Mock<IMailQueue> queue, Mock<IMailConfiguration> configuration, Mock<IMailSender> sender, Mock<ILog> logger)
            : base(queue.Object, configuration.Object, sender.Object, logger.Object)
        {
            Queue = queue;
            Configuration = configuration;
            Sender = sender;
            Logger = logger;
        }

        public static TestableMailSenderHost Create(Task queueStarter = null, Task queueStopper = null)
        {
            var queue = new Mock<IMailQueue>();
            var configuration = new Mock<IMailConfiguration>();
            var sender = new Mock<IMailSender>();
            var logger = new Mock<ILog>();
            if (queueStarter != null)
            {
                queue.Setup(mailQueue => mailQueue.StartListening()).Returns(queueStarter);
            }
            if (queueStopper != null)
            {
                queue.Setup(mailQueue => mailQueue.StopListening()).Returns(queueStopper);
            }
            return new TestableMailSenderHost(queue, configuration, sender, logger);
        }
    }
}