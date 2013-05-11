using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace Mailer
{
    public class MailSenderTests
    {
        [Fact]
        public void Mail_sender_starts_listening_queue_when_host_starts_and_changes_state_to_listening()
        {
            var emptyTask = new Task(() => { });
            var senderHost = TestableMailSenderHost.Create(emptyTask);
            senderHost.StartHost();
            emptyTask.Start();
            senderHost.Queue.Verify(queue => queue.StartListening(), Times.Once());
            Assert.True(senderHost.HostStatus == Status.Listening);
        }

        [Fact]
        public void Mail_sender_stops_queue_when_host_stops_and_changes_state_to_stopped()
        {
            var emptyTask = new Task(() => { });
            var senderHost = TestableMailSenderHost.Create(emptyTask, emptyTask);
            emptyTask.Start();

            senderHost.StartHost();
            senderHost.StopHost();
            
            Assert.True(senderHost.HostStatus == Status.Stopped);
        }

        [Fact]
        public void Mail_sender_changes_state_to_failed_and_logs_the_exception_when_queue_fails()
        {
            //is there a stability issue on this test?
            var errorTask = new Task(() => { throw new Exception(); });
            var senderHost = TestableMailSenderHost.Create(errorTask);

            senderHost.StartHost();
            try
            {
                errorTask.Start();
                errorTask.Wait();
            }
            catch
            {
                Thread.Sleep(3000); //couldn't find solution for async exception. This might be a pain in the 
            }
            
            senderHost.Logger.Verify(log => log.LogError(It.IsAny<Exception>()), Times.Once());
            Assert.True(senderHost.HostStatus == Status.Failed);
        }
    }
}