using Microsoft.Extensions.Caching.Memory;
using SiteProject.Models;
using SiteProject.ORM;

namespace SiteProject.Services;

public static class SessionManager
{
    private static readonly Dao<User> UserDao = DaoFactory.GetDao<User>();
    private static readonly MemoryCache Cache;
    static SessionManager()
    {
        Cache = new MemoryCache(new MemoryCacheOptions());
    }

    public static string CreateSessionId(int userId)
    {
        var sessionId = userId.GetHashCode().ToString() + DateTime.Now;
        var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(60*60));
        Cache.Set(sessionId, userId, cacheEntryOptions);
        return sessionId;
    }
    
    public static void RemoveSessionId(string sessionId) => Cache.Remove(sessionId);

    public static int GetUserId(string sessionId) => (int)(Cache.Get(sessionId) ?? 0);
    
    public static bool CheckSessionId(string sessionId) => GetUserId(sessionId) != 0;
}
