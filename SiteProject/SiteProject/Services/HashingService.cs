namespace SiteProject.Services;

public static class HashingService
{
    public static int HashPassword(string password)
    {
        return Hash("dcba" + password + "abcd");
    }

    private static int Hash(string password)//TODO use actually cryptographically effective hash
    {
        unchecked
        {
            return password.Aggregate(6449897, (current, symb) => current * symb + 9325913);
        }
    }
}

