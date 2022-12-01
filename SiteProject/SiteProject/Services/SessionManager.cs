using Microsoft.Extensions.Caching.Memory;

namespace SiteProject.Services;

public static class SessionManager
{
    private static readonly MemoryCache Cache;
    static SessionManager()
    {
        Cache = new MemoryCache(new MemoryCacheOptions());
    }

    public static string CreateSessionId(string userId)
    {
        var sessionId = userId.GetHashCode().ToString() + DateTime.Now;
        var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(60*60));
        Cache.Set(sessionId, userId, cacheEntryOptions);
        return sessionId;
    }
    
    public static void RemoveSessionId(string sessionId) => Cache.Remove(sessionId);

    public static string GetUserId(string sessionId) => Cache.Get(sessionId)?.ToString() ?? "0";
    
    public static bool CheckSessionId(string sessionId) => GetUserId(sessionId) != "0";
}
