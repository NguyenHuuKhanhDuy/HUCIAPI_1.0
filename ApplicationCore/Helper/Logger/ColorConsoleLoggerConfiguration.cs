using Microsoft.Extensions.Logging;

namespace ApplicationCore.Helper.Logger
{
    public sealed class ColorConsoleLoggerConfiguration
    {
        public int EventId { get; set; }

        public Dictionary<LogLevel, ConsoleColor> LogLevelToColorMap { get; set; } = new()
        {
            [LogLevel.Information] = ConsoleColor.Cyan,
            [LogLevel.Warning] = ConsoleColor.DarkYellow,
            [LogLevel.Error] = ConsoleColor.Red
        };
    }
}
