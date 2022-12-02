using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using SiteProject.ActionResults;
using SiteProject.Attributes;
using SiteProject.Services;

namespace SiteProject;

public class ServerResponse
{
    public readonly byte[]? Buffer;
    public readonly string? ContentType;
    public readonly string? RedirectionTarget;
    public CookieCollection? Cookies;
    public HttpStatusCode? Status;

    public ServerResponse(string path, HttpListenerRequest request)
    {
        RedirectionTarget = "";
        if (!Directory.Exists(path))
        {
            (Status, Buffer, ContentType) = (HttpStatusCode.NotFound,
                Encoding.UTF8.GetBytes($"Directory {path} does not exist"), "text/plain");
            return;
        }

        var buffer = GetFile(path + request.RawUrl?.Replace("%20", " "));
        var contentType = GetContentType(request.RawUrl);
        if (buffer.Length != 0)
        {
            Buffer = buffer;
            ContentType = contentType;
            Status = HttpStatusCode.OK;
            return;
        }

        TryHandleController(request, out RequestResult result);
        Status = (HttpStatusCode) result.StatusCode;
        ContentType = result.ContentType;
        RedirectionTarget = result.RedirectionTarget;
        Buffer = result.Buffer;
        Cookies = result.Cookies;
    }

    private static byte[] GetFile(string filePath)
    {
        if (Directory.Exists(filePath))
        {
            filePath += "/index.html";
            if (File.Exists(filePath))
                return File.ReadAllBytes(filePath);
        }

        if (File.Exists(filePath))
            return File.ReadAllBytes(filePath);
        return Array.Empty<byte>();
    }

    private static bool TryHandleController(HttpListenerRequest request, out RequestResult result)
    {
        result = new RequestResult(404, "text/plain", "Not found".ToByteArray());
        if (request.Url!.Segments.Length < 2) return false;
        using var sr = new StreamReader(request.InputStream, request.ContentEncoding);
        var controllerName = request.Url.Segments[1].Replace("/", "");
        var cookie = request.Cookies
            .FirstOrDefault(cookie => cookie.Name == "SessionId");
        var userId = cookie != null ? CookieManager.GetUserIdFromCookie(cookie) : "0";
        var strParams = QueryParser
            .Parse(sr.ReadToEnd())
            .Select(kv => kv.Value)
            .Append(userId)
            .ToArray();

        var assembly = Assembly.GetExecutingAssembly();
        var controller = assembly.GetTypes()
            .Where(t => Attribute.IsDefined(t, typeof(ApiController)))
            .FirstOrDefault(t => string.Equals(
                (t.GetCustomAttribute(typeof(ApiController)) as ApiController)?.ModelUri,
                controllerName,
                StringComparison.CurrentCultureIgnoreCase));
        var method = controller?.GetMethods()
            .FirstOrDefault(t => t.GetCustomAttributes(true)
                .Any(attr => attr.GetType().Name == $"Http{request.HttpMethod}"
                             && Regex.IsMatch(request.RawUrl ?? "",
                                 attr.GetType()
                                     .GetField("MethodUri")?
                                     .GetValue(attr)?.ToString() ?? "")));
        if (method is null) return false;
        var queryParams = method.GetParameters()
            .Select((p, i) => Convert.ChangeType(strParams[i], p.ParameterType))
            .ToArray();
        object? ret;
        if (controller.IsAbstract)
            ret = method.Invoke(null, queryParams);
        else
            ret = method.Invoke(controller.GetConstructors()[0].Invoke(Array.Empty<object>()), queryParams);
        if (ret is RequestResult res)
            result = res;
        return true;
    }

    private static string GetContentType(string path)
    {
        var ext = path.Contains('.') ? path.Split('.')[^1] : "html";
        return ContentTypes.ContainsKey(ext) ? ContentTypes[ext] : "text/plain";
    }

    private static readonly Dictionary<string, string> ContentTypes = new()
    {
        {"txt", "text/plain"},
        {"jpg", "image/jpeg"},
        {"png", "image/png"},
        {"gif", "image/gif"},
        {"svg", "image/svg+xml"},
        {"css", "text/css"},
        {"html", "text/html"}
    };
}