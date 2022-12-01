using System.Net;
using SiteProject.Models;
using SiteProject.ORM;

namespace SiteProject.Services;

public static class CookieManager
{
    private static readonly Dao<User> UserDao = DaoFactory.GetDao<User>();
    public static Cookie GetCookie(string login)
    {
        var userId = UserDao.SelectBy("Login", login).First().Id;
        var sessionId = SessionManager.CreateSessionId(userId);
        return new Cookie("SessionId", sessionId);
    }
}