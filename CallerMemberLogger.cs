using System;
using System.Runtime.CompilerServices;
using System.Net;
using System.IO;

namespace Serilog.Sinks.Http.Logger
{
    public abstract class CallerMemberLogger : ILog, IDisposable
    {
        protected ILogger _logger;
        public ILogger SerilogLogger => _logger;

        public void Debug(string format, object[] args = null, [CallerMemberName] string method = "", [CallerFilePath] string file = "", [CallerLineNumber] int linenr = 0)
        {
            _logger.ForHere(method, file, linenr).Debug(format, args);
        }

        public void Error(string format, object[] args = null, [CallerMemberName] string method = "", [CallerFilePath] string file = "", [CallerLineNumber] int linenr = 0)
        {
            _logger.ForHere(method, file, linenr).Error(format, args);
        }

        public void Error(string msg, HttpWebResponse httpResponse, object[] args = null, [CallerMemberName] string method = "", [CallerFilePath] string file = "", [CallerLineNumber] int linenr = 0)
        {
            Error(msg, args, method, file, linenr);
            Error(httpResponse.StatusDescription, null, method, file, linenr);
            Error(new StreamReader(httpResponse.GetResponseStream()).ReadToEnd(), null, method, file, linenr);
        }

        public void Information(string format, object[] args = null, [CallerMemberName] string method = "", [CallerFilePath] string file = "", [CallerLineNumber] int linenr = 0)
        {
            _logger.ForHere(method, file, linenr).Information(format, args);
        }

        public void Warning(string format, object[] args = null, [CallerMemberName] string method = "", [CallerFilePath] string file = "", [CallerLineNumber] int linenr = 0)
        {
            _logger.ForHere(method, file, linenr).Warning(format, args);
        }

        public void Dispose()
        {
            ((IDisposable)_logger).Dispose();
        }

        public static object[] BuildArgs(params object[] args)
        {
            return args;
        }
    }

    public static class LoggerExtensions
    {
        public static ILogger ForHere(this ILogger logger, string method, string file, int linenr)
        {
            var lio = file.LastIndexOf('/');
            if (lio == -1)
                lio = file.LastIndexOf('\\');

            return logger
                .ForContext("file", file.Substring(lio + 1))
                .ForContext("method", method)
                .ForContext("lnr", linenr);
        }
    }

}