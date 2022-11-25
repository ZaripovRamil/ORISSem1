using SiteProject.Attributes;
using SiteProject.DataTransfering;
using SiteProject.Models;
using SiteProject.ORM;

namespace SiteProject.Controllers;

[ApiController("/login")]
public class LoginController
{
    private static Dao<User> userDao = DaoFactory.GetDao<User>();
    private static LoginValidationResult ValidateLogin(string login, string password)
    {
        var user = userDao.SelectBy("Login", login).FirstOrDefault();
        if (user == null) return new LoginValidationResult("Invalid login");
        if (!IsValidPassword(user, password)) return new LoginValidationResult("InvalidPassword");
        return new LoginValidationResult(user.Role);
    }

    private static bool IsValidPassword(User user, string password)
    {
        return Password.HashPassword(password) == user.Password;
    }
}