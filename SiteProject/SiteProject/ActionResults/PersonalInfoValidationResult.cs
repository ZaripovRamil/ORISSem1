namespace SiteProject.ActionResults;

public class PersonalInfoValidationResult
{
    public PersonalInfoValidationResult(string message)
    {
        IsValid = false;
        Message = message;
    }

    public PersonalInfoValidationResult(int id)
    {
        IsValid = false;
        UserId = id;
    }


    public bool IsValid { get; set; }
    public string Message { get; set; }
    public int UserId { get; set; }
}