using System;
using System.Threading.Tasks;

namespace Mailer
{
    public class MailSenderHost : IMailSenderHost
    {
        private readonly IMailQueue _queue;
        private readonly IMailConfiguration _configuration;
        private readonly IMailSender _sender;
        private readonly ILog _logger;

        public Status HostStatus { get; private set; }

        public MailSenderHost(IMailQueue queue, IMailConfiguration configuration, IMailSender sender, ILog logger)
        {
            _queue = queue;
            _configuration = configuration;
            _sender = sender;
            _logger = logger;
        }

        public Task StartHost()
        {
            return _queue.StartListening()
                .Then<object>(() => HostStatus = Status.Listening)
                .Catch(LogError);
        }

        public Task StopHost()
        {
            return _queue.StopListening()
                         .Then<object>(() => HostStatus = Status.Stopped)
                         .Catch(LogError);
        }

        private void LogError(Exception error)
        {
            HostStatus = Status.Failed;
            _logger.LogError(error);
        }
    }
}