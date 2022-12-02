using SiteProject.ActionResults;
using SiteProject.Attributes;
using SiteProject.Models;
using SiteProject.ORM;

namespace SiteProject.Controllers;

public abstract class RoleController
{
    protected static readonly Dao<User> UserDao = DaoFactory.GetDao<User>();
    protected abstract bool IsCorrectRole(int userId);
    public static Role GetRole(int userId) => UserDao.SelectNotNullById(userId).Role;
    protected abstract bool IsInfoFilled(int userId);
    
    public virtual RequestResult GetUserMenu(int userId)
    {
        if (!IsCorrectRole(userId)) return RedirectToCorrectRole(userId);
        if (!IsInfoFilled(userId)) return RedirectToFillingInfo();
        return OpenView(userId);
    }

    public static RequestResult RedirectToCorrectRole(int userId)
    {
        return new RequestResult("http://localhost:6083/" + GetRole(userId));
    }

    protected static RequestResult RedirectToFillingInfo()
    {
        return new RequestResult("http://localhost:6083/" + "info");
    }

    protected abstract RequestResult OpenView(int userId);
}