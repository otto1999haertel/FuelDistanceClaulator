using System.Collections.Concurrent;

public class ApiThrottle
{
    private readonly TimeSpan _defaultInterval = TimeSpan.FromSeconds(1); // Standardintervall (1 Anfrage pro Sekunde)
    private readonly ConcurrentDictionary<string, DateTime> _lastCallTimes = new ConcurrentDictionary<string, DateTime>();

    public async Task<T> ExecuteWithThrottle<T>(string apiKey, Func<Task<T>> apiCall, TimeSpan? interval = null)
    {
        var intervalToUse = interval ?? _defaultInterval;
        var timeSinceLastCall = DateTime.Now - _lastCallTimes.GetOrAdd(apiKey, DateTime.MinValue);

        if (timeSinceLastCall < intervalToUse)
        {
            // Warten, bis der Mindestzeitraum abgelaufen ist
            await Task.Delay(intervalToUse - timeSinceLastCall);
        }

        // Führe den API-Aufruf aus
        _lastCallTimes[apiKey] = DateTime.Now;
        return await apiCall();
    }
}
