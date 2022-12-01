using SiteProject.ActionResults;
using SiteProject.Attributes;
using SiteProject.Models;
using SiteProject.ORM;
using SiteProject.Services;

namespace SiteProject.Controllers;

[ApiController("registration")]
public static class RegistrationController
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
}