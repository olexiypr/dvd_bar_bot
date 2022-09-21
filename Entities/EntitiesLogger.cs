using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace DvdBarBot.Entities;

public static class EntitiesLogger
{
    public static Logger Logger => new LoggerConfiguration()
        .MinimumLevel
        .Override("Microsoft", LogEventLevel.Information)
        .WriteTo.File("entitiesLogs.log", rollingInterval: RollingInterval.Day)
        .CreateLogger();
}