using System.Net;

namespace SiteProject.ActionResults;

public class RequestResult
{
    public RequestResult(string redirectionTarget)
    {
        StatusCode = 302;
        RedirectionTarget = redirectionTarget;
    }

    public RequestResult(int statusCode,string contentType, byte[] buffer)
    {
        StatusCode = statusCode;
        ContentType = contentType;
        Buffer = buffer;
    }

    public int StatusCode { get; set; }
    public string RedirectionTarget { get; set; }
    public string ContentType { get; set; }
    public byte[]Buffer { get; set; }
    public CookieCollection Cookies { get; set; }
}