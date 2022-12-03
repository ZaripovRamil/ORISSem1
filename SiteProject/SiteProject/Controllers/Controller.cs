using SiteProject.ActionResults;

namespace SiteProject.Controllers;

public abstract class Controller
{
    protected abstract RequestResult OpenView(string message, int userId);
}