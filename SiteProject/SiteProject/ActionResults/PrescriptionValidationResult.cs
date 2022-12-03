namespace SiteProject.ActionResults;

public class PrescriptionValidationResult
{
    public bool IsValid { get; set; }
    public string Message { get; set; }
    public int PrescriptionId;

    public PrescriptionValidationResult(string message)
    {
        IsValid = false;
        Message = message;
    }

    public PrescriptionValidationResult(int id)
    {
        IsValid = true;
        PrescriptionId = id;
    }
}