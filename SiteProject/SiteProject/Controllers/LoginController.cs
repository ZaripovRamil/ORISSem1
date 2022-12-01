﻿using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Text;
using Scriban;
using SiteProject.ActionResults;
using SiteProject.Attributes;
using SiteProject.Models;
using SiteProject.ORM;
using SiteProject.Services;

namespace SiteProject.Controllers;

[ApiController("login")]
public static class LoginController
{
    private static readonly Dao<User> UserDao = DaoFactory.GetDao<User>();
    private static readonly Dao<MyCookie> CookieDao = DaoFactory.GetDao<MyCookie>();
    private static LoginValidationResult ValidateLogin(string login, string password)
    {
        var user = UserDao.SelectBy("Login", login).FirstOrDefault();
        if (user == null) return new LoginValidationResult("invalidlogin");
        if (!IsValidPassword(user, password)) return new LoginValidationResult("invalidpassword");
        return new LoginValidationResult(user.Role);
    }

    [HttpPOST]
    public static RequestResult LoginAttempt(string login, string password, string remember)
    {
        var res = ValidateLogin(login, password);
        if (res.IsValid)
        {
            var cookie = CookieManager.ProvideCookie(login, remember == "true");
            return new RequestResult("http://localhost:6083/" + res.UserRole)
                {Cookies = new CookieCollection{cookie}};
        }

        return new RequestResult("http://localhost:6083/login/" + res.Message);
    }

    [HttpGET("invalidlogin")]
    public static RequestResult OpenInvLoginView(int userId)
        => OpenView("Invalid login", userId);

    [HttpGET("invalidpassword")] //TODO: remove this, use JS
    public static RequestResult OpenInvPasswordView(int userId)
        => OpenView("Invalid password", userId);

    [HttpGET]
    public static RequestResult OpenView(int userId)
        => OpenView("", userId);

    private static RequestResult OpenView(string message, int userId)
    {
        if (userId != 0) return RoleController.RedirectToCorrectRole(userId);
        var template = Template.Parse(File.ReadAllText("Views/login.sbnhtml"));
        var res = Encoding.UTF8.GetBytes(template.Render(new {message = message}));
        return new RequestResult(200, "text/html", res);
    }

    private static bool IsValidPassword(User user, string password)
    {
        Console.WriteLine(user.Password);
        Console.WriteLine(HashingService.HashPassword(password));
        return HashingService.HashPassword(password) == user.Password;
    }
}