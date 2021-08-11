using AuroraCore.Application.Interfaces;
using Sentry;
using System;

namespace AuroraCore.Infrastructure.Services
{
    public class SentryLogger : ILogger
    {
        public SentryLogger()
        {
            SentrySdk.Init("https://44d9fa755a2e44bc9e8038079965a8a4@o564628.ingest.sentry.io/5705461");
        }

        public void Log(Exception exception)
        {
            SentrySdk.CaptureException(exception);
        }
    }
}