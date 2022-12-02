namespace SiteProject.ActionResults;

public class TreatmentCreatingResult
{
    public bool IsValid { get; set; }
    public string Message { get; set; }
    public int TreatmentId;

    public TreatmentCreatingResult(string message)
    {
        IsValid = false;
        Message = message;
    }

    public TreatmentCreatingResult(int id)
    {
        IsValid = true;
        TreatmentId = id;
    }
}