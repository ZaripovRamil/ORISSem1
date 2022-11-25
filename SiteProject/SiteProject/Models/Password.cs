namespace SiteProject.Models;

public static class Password
{
    public static int HashPassword(string password)
    {
        return ("dcba" + password + "abcd").GetHashCode();
    }
}