namespace SiteProject;

public static class QueryParser
{
    public static Dictionary<string, string> Parse(string query) => query.Split('&')
        .Where(s=>s!="")
        .Select(pair => pair.Split('='))
        .Select(pair => (pair[0], pair[1]))
        .ToDictionary(pair => pair.Item1, pair => pair.Item2);
}