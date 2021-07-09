using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions.Internal;
using System;
using System.Collections.Generic;

namespace CrudFunctions.Tests.Base
{
    public class ListLogger : ILogger
    {
        private readonly IList<string> _logs;

        public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;

        public bool IsEnabled(LogLevel logLevel) => false;

        public ListLogger()
        {
            _logs = new List<string>();
        }

        public void Log<TState>(LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            var message = formatter(state, exception);
            this._logs.Add(message);
        }
    }
}