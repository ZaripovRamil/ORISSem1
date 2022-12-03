using System.Net;
using System.Text;
using Scriban;
using SiteProject.ActionResults;
using SiteProject.Attributes;
using SiteProject.Models;
using SiteProject.ORM;
using SiteProject.Services;

namespace SiteProject.Controllers;

[ApiController("register")]
public class RegistrationController:Controller
{
    private static RegistrationResult ValidateRegistration(string login, string password, Role role)
    {
        var dao = DaoFactory.GetDao<User>();
        var user = dao.SelectBy("Login", login).FirstOrDefault();
        if (user != null) return new RegistrationResult("User with such name already exists");
        if (password.Length < 6)
            return new RegistrationResult("Your password should be at least 6 characters long");
        dao.Insert(new User(login, HashingService.HashPassword(password), role));
        return new RegistrationResult(dao.SelectBy("Login", login).First().Id);
    }
    
    [HttpGET]
    public RequestResult OpenView(int userId)
        => OpenView("", userId);
    
    [HttpPOST]
    public RequestResult RegistrationAttempt(string login, string password, string role, string remember)
    {
        var res = ValidateRegistration(login, password, RoleHandler.GetRole(role));
        if (!res.IsValid) return OpenView(res.Message, 0);
        var cookie = CookieManager.ProvideCookie(login, remember == "true");
        return new RequestResult("http://localhost:6083/info")
            {Cookies = new CookieCollection{cookie}};
    }


    protected override RequestResult OpenView(string message, int userId)
    {
        if (userId != 0) return RoleController.RedirectToCorrectRole(userId);
        var template = Template.Parse(File.ReadAllText("Views/registration.html"));
        var res = Encoding.UTF8.GetBytes(template.Render(new {message = message}));
        return new RequestResult(200, "text/html", res);
    }
}