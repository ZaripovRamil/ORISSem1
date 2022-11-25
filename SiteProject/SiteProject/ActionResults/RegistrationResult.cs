using SiteProject.Models;

namespace SiteProject.DataTransfering;

public class RegistrationResult
{
    public RegistrationResult(string message)
    {
        IsValid = false;
        Message = message;
    }
    
    public RegistrationResult(int id)
    {
        IsValid = false;
        UserId = id;
    }
    

    public bool IsValid { get; set; }
    public string Message { get; set; }
    public int UserId;
}