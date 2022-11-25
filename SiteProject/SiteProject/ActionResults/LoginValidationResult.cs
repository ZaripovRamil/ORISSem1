using SiteProject.Models;

namespace SiteProject.DataTransfering;

public class LoginValidationResult
{
    public LoginValidationResult(string message)
    {
        IsValid = false;
        Message = message;
    }
    
    public LoginValidationResult(Role role)
    {
        IsValid = false;
        UserRole = role;
    }
    

    public bool IsValid { get; set; }
    public string Message { get; set; }
    public Role UserRole;
}