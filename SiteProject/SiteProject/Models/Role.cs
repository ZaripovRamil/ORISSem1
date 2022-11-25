namespace SiteProject.Models;

public enum Role
{
    Medic, Client
}

public static class RoleHandler
{
    public static Role GetRole(string role)
        => role switch
        {
            "Medic" => Role.Medic,
            "Client" => Role.Client,
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
        };
}