using System.Collections.Generic;
using System.Linq;
using BepInEx.Logging;

namespace MonsterModifiers.Patches;

public static class ShaderLogFilter
{
    public static void Apply()
    {
        var existing = Logger.Listeners.ToList();
        Logger.Listeners.Clear();
        Logger.Listeners.Add(new FilteringListener(existing));
    }

    private sealed class FilteringListener : ILogListener
    {
        private readonly List<ILogListener> _inner;

        public FilteringListener(List<ILogListener> inner) => _inner = inner;

        public void LogEvent(object sender, LogEventArgs e)
        {
            if (e.Level == LogLevel.Warning &&
                e.Data?.ToString()?.StartsWith("Failed to find expected binary shader data") == true)
                return;
            foreach (var l in _inner) l.LogEvent(sender, e);
        }

        public void Dispose()
        {
            foreach (var l in _inner) l.Dispose();
        }
    }
}
