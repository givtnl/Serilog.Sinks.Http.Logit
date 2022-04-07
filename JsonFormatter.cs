using System.IO;
using Serilog.Events;
using Serilog.Formatting;
using Newtonsoft.Json.Linq;

namespace Serilog.Sinks.Http.LogitV2
{
    public class JsonFormatter : ITextFormatter
    {
        public void Format(LogEvent logEvent, TextWriter output)
        {
            JObject jsonObj = new JObject();
            foreach (var prop in logEvent.Properties)
                jsonObj[prop.Key] = prop.Value.ToString().Replace("\"", string.Empty);

            jsonObj["timestamp"] = logEvent.Timestamp.ToString("o");
            jsonObj["level"] = logEvent.Level.ToString();
            jsonObj["message"] = logEvent.MessageTemplate.Render(logEvent.Properties);
            if (logEvent.Exception != null)
            {
                jsonObj["exception.msg"] = logEvent.Exception.Message;
                jsonObj["exception.stacktrace"] = logEvent.Exception.StackTrace;
            }
            output.WriteLine(jsonObj.ToString(Newtonsoft.Json.Formatting.None));
        }
    }
}
