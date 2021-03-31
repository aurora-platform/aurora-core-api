using System;

namespace AuroraCore.Domain.Shared
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message)
        {
        }
    }
}
