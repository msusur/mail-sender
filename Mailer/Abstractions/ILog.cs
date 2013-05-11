using System;

namespace Mailer
{
    public interface ILog
    {
        void LogError(Exception error);
    }
}