namespace SiteProject.Attributes;

class HttpPOST : HttpRequest
{
    public HttpPOST(string methodUri = "") : base(methodUri) { }
}