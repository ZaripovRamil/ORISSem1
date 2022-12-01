using SiteProject.Attributes;

namespace SiteProject.Models;

public class MyCookie
{
    [Id("Id")]
    public int Id { get; set; }
    [ValueColumn("Name")]
    public string SessionId { get; set; }
    [ValueColumn("Value")]
    public string UserId { get; set; }

    [DbRecordCtor]
    public MyCookie(int id, string sessionId, string userId)
    {
        Id = id;
        SessionId = sessionId;
        UserId = userId;
    }
    
    public MyCookie(string sessionId, string userId)
    {
        SessionId = sessionId;
        UserId = userId;
    }
}