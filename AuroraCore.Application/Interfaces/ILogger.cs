using System;

namespace AuroraCore.Application.Interfaces
{
    public interface ILogger
    {
        void Log(Exception exception);
    }
}
