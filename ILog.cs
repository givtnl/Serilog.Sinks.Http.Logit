using System.Runtime.CompilerServices;
using System.Net;

namespace Serilog.Sinks.Http.LogitV2
{
    public interface ILog
    {
        ILogger SerilogLogger { get; }

        void Debug(string msg, object[] args = null, [CallerMemberName] string method = "", [CallerFilePath] string file = "", [CallerLineNumber] int linenr = 0);
        void Information(string msg, object[] args = null, [CallerMemberName] string method = "", [CallerFilePath] string file = "", [CallerLineNumber] int linenr = 0);
        void Warning(string msg, object[] args = null, [CallerMemberName] string method = "", [CallerFilePath] string file = "", [CallerLineNumber] int linenr = 0);
        void Error(string msg, object[] args = null, [CallerMemberName] string method = "", [CallerFilePath] string file = "", [CallerLineNumber] int linenr = 0);
        void Error(string msg, HttpWebResponse httpResponse, object[] args = null, [CallerMemberName] string method = "", [CallerFilePath] string file = "", [CallerLineNumber] int linenr = 0);
    }
}
