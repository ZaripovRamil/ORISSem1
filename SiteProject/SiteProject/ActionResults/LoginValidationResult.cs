using SiteProject.Models;

namespace SiteProject.ActionResults;

public class LoginValidationResult
{
    public LoginValidationResult(string message)
    {
        IsValid = false;
        Message = message;
    }

    public LoginValidationResult(Role role)
    {
        IsValid = true;
        UserRole = role;
    }


    public bool IsValid { get; set; }
    public string Message { get; set; }
    public Role UserRole;
}