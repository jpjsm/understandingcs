using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace LogRecord
{
    public enum LogType
    {
        Trace,
        Debug,
        Information,
        Warning,
        Error,
        Critical
    }

    public enum CallStatus
    {
        Success,
        ClientError,
        SystemError
    }

    public class LogRecord
    {
        private DateTime _timestamp = DateTime.UtcNow;

        [JsonProperty]
        public string TimeStamp => _timestamp.ToString("O");

        [JsonIgnore]
        internal LogType type { get; set; }

        [JsonProperty]
        public string Type => Enum.GetName(typeof(LogType), type);

        [JsonProperty]
        public string System { get; set; }

        [JsonProperty]
        public string Class { get; set; }

        [JsonProperty]
        public string FunctionName { get; set; }

        [JsonProperty]
        public string FunctionArguments { get; set; }

        [JsonProperty]
        public string HttpMethod { get; set; }

        [JsonProperty]
        public string HttpUri { get; set; }

        [JsonProperty]
        public string HttpBody { get; set; }

        [JsonProperty]
        public string Message { get; set; }

        [JsonProperty]
        public double DurationMilliSec { get; set; }

        [JsonIgnore]
        internal CallStatus status { get; set; }

        [JsonProperty]
        public string Status => Enum.GetName(typeof(CallStatus), status);

        [JsonIgnore]
        public Dictionary<string, string> Properties => this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Select(p => p).Where(p => p.Name != "Properties").ToDictionary(k => k.Name, k => k.GetValue(this) != null ? k.GetValue(this).ToString() : string.Empty);

        public LogRecord()
        {
            // default constructor no arguments
        }

        public string ToJson() => JsonConvert.SerializeObject(this);
    }
}
