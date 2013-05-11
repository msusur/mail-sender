using System.Threading.Tasks;

namespace Mailer
{
    public interface IMailQueue
    {
        Task StartListening();
        Task StopListening();
    }
}