using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Serilog.Core;
using Serilog.Formatting;
using Serilog.Events;
using Serilog.Configuration;

namespace Serilog.Sinks.Http
{
    public class HttpSink : ILogEventSink, IDisposable
    {
        string _url;
        string _apiKey;
        readonly HttpClient _httpClient;
        readonly ITextFormatter _formatter;

        public HttpSink(string url, string apiKey, HttpMessageHandler handler = null)
        {
            _formatter = new JsonFormatter();
            _url = url;
            _apiKey = apiKey;
            
            if (handler != null)
                _httpClient = new HttpClient(handler);
            else
                _httpClient = new HttpClient();
        }

        public void Emit(LogEvent logEvent)
        {
            var sb = new StringBuilder();
            _formatter.Format(logEvent, new StringWriter(sb));
            var data = sb.ToString().Replace("RenderedMessage", "message");

            Task.Factory.StartNew(async () => 
            {
                try
                {
                    var content = new StringContent(data);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    content.Headers.Add("ApiKey", _apiKey);
                    (await _httpClient.PostAsync(_url, content)).Dispose();
                } catch  { } /* This is a logging framework. We don't care about logs not succeeding */
            });
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }

    public static class HttpLoggerConfigurationExtensions
    {
        public static LoggerConfiguration HttpSink(this LoggerSinkConfiguration loggerConfiguration, string url, string apiKey, 
                HttpMessageHandler msgHandler = null, LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum)
        {
            var sink = new HttpSink(url, apiKey, msgHandler);
            return loggerConfiguration.Sink(sink, restrictedToMinimumLevel);
        } 
    }
}