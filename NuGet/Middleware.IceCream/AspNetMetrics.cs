using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Middleware.IceCream.Metrics
{
    public class AspNetMetrics
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly ILogger _metricsLogger;

        private static readonly string _serviceName = "aspnet-middleware";
        private static readonly string _serviceVersion = "0.0.1";
        private static Meter meter = new Meter(_serviceName, _serviceVersion);
        private Dictionary<string, Counter<int>> requestCounters;
        private Dictionary<string, Counter<long>> elapsedTimeCounters;
        private Dictionary<string, int> observableRequestCounters;
        private Dictionary<string, int> requestGauges;
        private Dictionary<string, int> elapsedTimeGauges;
        private Dictionary<string, Histogram<int>> requestHistograms;
        private Dictionary<string, int> requestCountersLocal;
        private Dictionary<string, int> elapsedTimeCountersLocal;

        private void InitMetrics()
        {
            Sdk.CreateMeterProviderBuilder()
                .AddMeter(_serviceName)
                .AddOtlpExporter(options =>
                {
                    options.Protocol = OtlpExportProtocol.Grpc;
                    options.Endpoint = new Uri("http://localhost:4317");
                })
                .Build();
            requestCounters = new Dictionary<string, Counter<int>>();
            elapsedTimeCounters = new Dictionary<string, Counter<long>>();
            observableRequestCounters = new Dictionary<string, int>();
            requestGauges = new Dictionary<string, int>();
            elapsedTimeGauges = new Dictionary<string, int>();
            requestHistograms = new Dictionary<string, Histogram<int>>();
            requestCountersLocal = new Dictionary<string, int>();
            elapsedTimeCountersLocal = new Dictionary<string, int>();
        }

        public AspNetMetrics(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger("logging");
            _metricsLogger = loggerFactory.CreateLogger<AspNetMetrics>();
            InitMetrics();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var utcNow = DateTime.UtcNow;
            var localNow = DateTime.Now;
            var routevals = context.Request.RouteValues;
            var fname = routevals["action"]?.ToString();

            if (fname == null)
            {
                fname = "InvalidRequest";
            }
            if (!requestCounters.ContainsKey(fname))
            {
                requestCounters[fname] = meter.CreateCounter<int>($"{fname}_requests_counter");
            }
            if (!elapsedTimeCounters.ContainsKey(fname))
            {
                elapsedTimeCounters[fname] = meter.CreateCounter<long>($"{fname}_elapsedtime_counter");
            }
            if (!observableRequestCounters.ContainsKey(fname))
            {
                observableRequestCounters[fname] = 0;
                meter.CreateObservableCounter<int>($"{fname}_requests_counter_observable", () => observableRequestCounters[fname]);
            }
            if (!requestGauges.ContainsKey(fname))
            {
                requestGauges[fname] = 0;
                meter.CreateObservableGauge<int>($"{fname}_requests_gauge", () => requestGauges[fname]);
            }
            if (!elapsedTimeGauges.ContainsKey(fname))
            {
                elapsedTimeGauges[fname] = 0;
                meter.CreateObservableGauge<int>("${ fname}_elapsedtime_gauge", () => elapsedTimeGauges[fname]);
            }
            if (!requestHistograms.ContainsKey(fname))
            {
                requestHistograms[fname] = meter.CreateHistogram<int>($"{fname}_requests_histogram");
            }
            if (!requestCountersLocal.ContainsKey(fname))
            {
                requestCountersLocal[fname] = 0;
            }
            if (!elapsedTimeCountersLocal.ContainsKey(fname))
            {
                elapsedTimeCountersLocal[fname] = 0;
            }

            var dimensions = new TagList()
            {
                {"fname", $"{fname}" },
                {"Scheme", $"{context.Request.Scheme}" },
                {"Verb", $"{context.Request.Method}" },
                {"Host", $"{context.Request.Host}" },
                {"Path", $"{context.Request.Path}" },
                {"Query_Params", $"{context.Request.QueryString}" },
                {"utc_year", $"{utcNow.Year}" },
                {"utc_month", $"{utcNow.Month}" },
                {"utc_day", $"{utcNow.Day}" },
                {"utc_weekday", $"{utcNow.DayOfWeek}" },
                {"utc_hour", $"{utcNow.Hour}" },
                {"utc_minute", $"{utcNow.Minute}" },
                {"utc_second", $"{utcNow.Second}" },
                {"local_year", $"{localNow.Year}" },
                {"local_month", $"{localNow.Month}" },
                {"local_day", $"{localNow.Day}" },
                {"local_weekday", $"{localNow.DayOfWeek}" },
                {"local_hour", $"{localNow.Hour}" },
                {"local_minute", $"{localNow.Minute}" },
                {"local_second", $"{localNow.Second}" },
            };

            var watch = Stopwatch.StartNew();
            _logger.LogInformation($"Beginning {fname}");
            try
            {
                await _next(context);
                dimensions.Add(new("Http_code", context.Response.StatusCode));
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Error inside of {fname}");

                dimensions.Add(new("error_msg", ex.Message));
                dimensions.Add(new("error_trace", ex.StackTrace));
                dimensions.Add(new("http_code", 500));
            }
            watch.Stop();
            _logger.LogInformation($"{fname} finished successfully");
            try
            {
                UpdateMetrics(fname, dimensions, watch.ElapsedMilliseconds);
            }
            catch(Exception ex)
            {
                _logger.LogCritical(ex, "Critical error in update metrics");
            }

        }

        private void UpdateMetrics(string fname, TagList dimensions, long run_time)
        {
            // OTEL Instruments
            elapsedTimeCounters[fname].Add(run_time, dimensions);
            requestCounters[fname].Add(1, dimensions);
            observableRequestCounters[fname] += 1;
            requestGauges[fname] += 1;
            elapsedTimeGauges[fname] += (int)run_time;
            requestHistograms[fname].Record(1, dimensions);
            // Local Counters
            requestCountersLocal[fname]++;
            elapsedTimeCountersLocal[fname] += (int)run_time;
            _logger.LogDebug("{@dimensions}", dimensions);
            // Log instrument info
            LogInstrument(dimensions, "counter", $"{fname}_requests_counter", requestCountersLocal[fname]);
            LogInstrument(dimensions, "counter", $"{fname}_elapsedtime_counter", elapsedTimeCountersLocal[fname]);
            LogInstrument(dimensions, "gauge", $"{fname}_requests_gauge", requestCountersLocal[fname]);
            LogInstrument(dimensions, "gauge", $"{fname}_elapsedtime_gauge", elapsedTimeCountersLocal[fname]);
        }
        
        private void LogInstrument(TagList dimensions, string instrumentType, string instrumentName, int count)
        {
            _metricsLogger.LogInformation("{@instrument}",
                new
                {
                    instrumentType = instrumentType,
                    instrumentName = instrumentName,
                    instrumentCount = count,
                    dimensions = dimensions
                });
        }
    }
}