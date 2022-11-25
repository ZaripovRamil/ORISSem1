namespace SiteProject.DataTransfering;

public class RecipeRequestCreatingResult
{
    public bool IsValid { get; set; }
    public string Message { get; set; }
    public int RecipeRequestId;

    public RecipeRequestCreatingResult(string message)
    {
        IsValid = false;
        Message = message;
    }
    public RecipeRequestCreatingResult(int id)
    {
        IsValid = true;
        RecipeRequestId = id;
    }
}