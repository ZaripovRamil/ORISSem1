using System.Net;
using SiteProject.Models;
using SiteProject.ORM;

namespace SiteProject.Services;

public static class CookieManager
{
    private static readonly Dao<User> UserDao = DaoFactory.GetDao<User>();
    private static readonly Dao<MyCookie> CookieDao = DaoFactory.GetDao<MyCookie>();

    public static Cookie ProvideCookie(string login, bool save)
    {
        var userId = UserDao.SelectBy("Login", login).First().Id.ToString();
        var sessionId = SessionManager.CreateSessionId(userId);
        if (save)
            CookieDao.Insert(new MyCookie(sessionId, userId));
        return new Cookie("SessionId", sessionId);
    }

    public static string GetUserIdFromCookie(Cookie cookie)
    {
        var cookieInDb = CookieDao.SelectBy("Name", cookie.Value).FirstOrDefault();
        if (cookieInDb != null) return cookieInDb.UserId;
        return SessionManager.GetUserId(cookie.Value);
    }
}