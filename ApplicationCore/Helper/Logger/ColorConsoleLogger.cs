using Microsoft.Extensions.Logging;

namespace ApplicationCore.Helper.Logger
{
    public sealed class ColorConsoleLogger : ILogger
    {
        private readonly string _name;
        private readonly Func<ColorConsoleLoggerConfiguration> _getCurrentConfig;

        public ColorConsoleLogger(
            string name,
            Func<ColorConsoleLoggerConfiguration> getCurrentConfig) =>
            (_name, _getCurrentConfig) = (name, getCurrentConfig);

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

        public bool IsEnabled(LogLevel logLevel) =>
            _getCurrentConfig().LogLevelToColorMap.ContainsKey(logLevel);

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            ColorConsoleLoggerConfiguration config = _getCurrentConfig();
            if (config.EventId == 0 || config.EventId == eventId.Id)
            {
                ConsoleColor originalColor = Console.ForegroundColor;
                string time = DateTime.UtcNow.AddHours(7).ToString("HH:mm dd/MM/yyyy");
                Console.ForegroundColor = config.LogLevelToColorMap[logLevel];
                Console.WriteLine($"[{time}: {logLevel}]");

                Console.ForegroundColor = originalColor;
                Console.Write($"     {_name} - ");

                Console.ForegroundColor = config.LogLevelToColorMap[logLevel];
                Console.Write($"{formatter(state, exception)}");

                Console.ForegroundColor = originalColor;
                Console.WriteLine();
            }
        }
    }
}
