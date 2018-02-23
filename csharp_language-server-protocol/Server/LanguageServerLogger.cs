using System;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace OmniSharp.Extensions.LanguageServer.Server
{
    class LanguageServerLogger : ILogger
    {
        private LanguageServer _languageServer;

        public LanguageServerLogger(LanguageServer languageServer)
        {
            _languageServer = languageServer;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            // TODO
            return new ImmutableDisposable();
        }

        public bool IsEnabled(LogLevel logLevel) => logLevel >= _languageServer.MinimumLogLevel;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (logLevel < _languageServer.MinimumLogLevel)
                return;

            if (TryGetMessageType(logLevel, out var messageType))
            {
                _languageServer.Log(new LogMessageParams()
                {
                    Type = messageType,
                    Message = formatter(state, exception)
                });
            }
        }

        private bool TryGetMessageType(LogLevel logLevel, out MessageType messageType)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                case LogLevel.Error:
                    messageType = MessageType.Error;
                    return true;
                case LogLevel.Warning:
                    messageType = MessageType.Warning;
                    return true;
                case LogLevel.Information:
                    messageType = MessageType.Info;
                    return true;
                case LogLevel.Debug:
                case LogLevel.Trace:
                    messageType = MessageType.Info;
                    return true;
            }
            messageType = MessageType.Log;
            return false;
        }
    }
}
