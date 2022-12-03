namespace SiteProject.ActionResults;

public class PrescriptionRequestValidationResult
{
    public bool IsValid { get; set; }
    public string Message { get; set; }
    public int PresriptionRequestId;

    public PrescriptionRequestValidationResult(string message)
    {
        IsValid = false;
        Message = message;
    }

    public PrescriptionRequestValidationResult(int id)
    {
        IsValid = true;
        PresriptionRequestId = id;
    }
}