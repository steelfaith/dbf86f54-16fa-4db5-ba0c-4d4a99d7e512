using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using log4net;
using log4net.Core;
using log4net.Util;

namespace Common
{
    /// <summary>
    /// currently we arent preserving thread id from caller
    /// for debugging purposes I might add it in later but this is much
    /// cleaner than the previous implmentation
    /// </summary>
    public class AsyncLogger
    {
        private readonly object _lock = new object();
        private readonly List<Action> _messages = new List<Action>();
        private readonly ILog _logger;
        private readonly Timer _timer;
        private readonly bool _logOriginatingThread;

        public AsyncLogger(ILog logger, bool logOriginatingThread)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _logOriginatingThread = logOriginatingThread;
            _timer = new Timer(WriteMessagesToTargets, null, 1000, 0);
            _logger = logger;
        }

        private void WriteMessagesToTargets(object state)
        {
            List<Action> clonedList = new List<Action>();
            lock (_lock)
            {
                if (_messages.Count > 0)
                {
                    clonedList = new List<Action>(_messages);
                    _messages.Clear();
                }
            }

            foreach (var item in clonedList)
                item.Invoke();

            _timer.Change(1000, 0);
        }

        #region ILog

        public void Debug(object message)
        {
            lock (_lock)
                _messages.Add(() => _logger.Debug(message));
        }

        public void Debug(object message, Exception exception)
        {
            lock (_lock)
                _messages.Add(() => _logger.Debug(message, exception));
        }

        public void DebugFormat(string format, params object[] args)
        {
            if (IsDebugEnabled)
                lock (_lock)
                    _messages.Add(() => _logger.DebugFormat(format, args));
        }

        public void DebugFormat(string format, object arg0)
        {
            if (IsDebugEnabled)
                lock (_lock)
                    _messages.Add(() => _logger.DebugFormat(format, arg0));
        }

        public void DebugFormat(string format, object arg0, object arg1)
        {
            if (IsDebugEnabled)
                lock (_lock)
                    _messages.Add(() => _logger.DebugFormat(format, arg0, arg1));
        }

        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            if (IsDebugEnabled)
                lock (_lock)
                    _messages.Add(() => _logger.DebugFormat(format, arg0, arg1, arg2));
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (IsDebugEnabled)
                lock (_lock)
                    _messages.Add(() => _logger.DebugFormat(provider, format, args));
        }

        public void Info(object message)
        {
            lock (_lock)
                _messages.Add(() => _logger.Info(message));
        }

        public void Info(object message, Exception exception)
        {
            lock (_lock)
                _messages.Add(() => _logger.Info(message, exception));
        }

        public void InfoFormat(string format, params object[] args)
        {
            if (IsInfoEnabled)
                lock (_lock)
                    _messages.Add(() => _logger.InfoFormat(format, args));
        }

        public void InfoFormat(string format, object arg0)
        {
            if (IsInfoEnabled)
                lock (_lock)
                    _messages.Add(() => _logger.InfoFormat(format, arg0));
        }

        public void InfoFormat(string format, object arg0, object arg1)
        {
            if (IsInfoEnabled)
                lock (_lock)
                    _messages.Add(() => _logger.InfoFormat(format, arg0, arg1));
        }

        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            if (IsInfoEnabled)
                lock (_lock)
                    _messages.Add(() => _logger.InfoFormat(format, arg0, arg1, arg2));
        }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (IsInfoEnabled)
                lock (_lock)
                    _messages.Add(() => _logger.InfoFormat(provider, format, args));
        }

        public void Warn(object message)
        {
            lock (_lock)
                _messages.Add(() => _logger.Warn(message));
        }

        public void Warn(object message, Exception exception)
        {
            lock (_lock)
                _messages.Add(() => _logger.Warn(message, exception));
        }

        public void WarnFormat(string format, params object[] args)
        {
            if (IsWarnEnabled)
                lock (_lock)
                    _messages.Add(() => _logger.WarnFormat(format, args));
        }

        public void WarnFormat(string format, object arg0)
        {
            if (IsWarnEnabled)
                lock (_lock)
                    _messages.Add(() => _logger.WarnFormat(format, arg0));
        }

        public void WarnFormat(string format, object arg0, object arg1)
        {
            if (IsWarnEnabled)
                lock (_lock)
                    _messages.Add(() => _logger.WarnFormat(format, arg0, arg1));
        }

        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            if (IsWarnEnabled)
                lock (_lock)
                    _messages.Add(() => _logger.WarnFormat(format, arg0, arg1, arg2));
        }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (IsWarnEnabled)
                lock (_lock)
                    _messages.Add(() => _logger.WarnFormat(provider, format, args));
        }

        public void Error(object message)
        {
            lock (_lock)
                _messages.Add(() => _logger.Error(message));
        }

        public void Error(object message, Exception exception)
        {
            lock (_lock)
                _messages.Add(() => _logger.Error(message, exception));
        }

        public void ErrorFormat(string format, params object[] args)
        {
            if (IsErrorEnabled)
                lock (_lock)
                    _messages.Add(() => _logger.ErrorFormat(format, args));
        }

        public void ErrorFormat(string format, object arg0)
        {
            if (IsErrorEnabled)
                lock (_lock)
                    _messages.Add(() => _logger.ErrorFormat(format, arg0));
        }

        public void ErrorFormat(string format, object arg0, object arg1)
        {
            if (IsErrorEnabled)
                lock (_lock)
                    _messages.Add(() => _logger.ErrorFormat(format, arg0, arg1));
        }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            if (IsErrorEnabled)
                lock (_lock)
                    _messages.Add(() => _logger.ErrorFormat(format, arg0, arg2));
        }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (IsErrorEnabled)
                lock (_lock)
                    _messages.Add(() => _logger.ErrorFormat(format, args));
        }

        public void Fatal(object message)
        {
            lock (_lock)
                _messages.Add(() => _logger.Fatal(message));
        }

        public void Fatal(object message, Exception exception)
        {
            lock (_lock)
                _messages.Add(() => _logger.Fatal(message, exception));
        }

        public void FatalFormat(string format, params object[] args)
        {
            if (IsFatalEnabled)
                lock (_lock)
                    _messages.Add(() => _logger.FatalFormat(format, args));
        }

        public void FatalFormat(string format, object arg0)
        {
            if (IsFatalEnabled)
                lock (_lock)
                    _messages.Add(() => _logger.FatalFormat(format, arg0));
        }

        public void FatalFormat(string format, object arg0, object arg1)
        {
            if (IsFatalEnabled)
                lock (_lock)
                    _messages.Add(() => _logger.FatalFormat(format, arg0, arg1));
        }

        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            if (IsFatalEnabled)
                lock (_lock)
                    _messages.Add(() => _logger.FatalFormat(format, arg0, arg1, arg2));
        }

        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (IsFatalEnabled)
                lock (_lock)
                    _messages.Add(() => _logger.FatalFormat(provider, format, args));
        }

        public bool IsDebugEnabled => _logger.IsDebugEnabled;
        public bool IsInfoEnabled => _logger.IsInfoEnabled;
        public bool IsWarnEnabled => _logger.IsWarnEnabled;
        public bool IsErrorEnabled => _logger.IsErrorEnabled;
        public bool IsFatalEnabled => _logger.IsFatalEnabled;

        #endregion
    }
}