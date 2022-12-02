namespace SiteProject.ActionResults;

public class RegistrationResult
{
    public RegistrationResult(string message)
    {
        IsValid = false;
        Message = message;
    }

    public RegistrationResult(int id)
    {
        IsValid = true;
        UserId = id;
    }


    public bool IsValid { get; set; }
    public string Message { get; set; }
    public int UserId { get; set; }
}