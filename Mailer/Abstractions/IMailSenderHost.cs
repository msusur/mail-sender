using System.Threading.Tasks;

namespace Mailer
{
    public interface IMailSenderHost
    {
        Status HostStatus { get; }

        Task StartHost();
        Task StopHost();
    }
}