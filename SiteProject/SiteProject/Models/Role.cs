namespace SiteProject.Models;

public enum Role
{
    Invalid,
    Doctor,
    Patient
}

public static class RoleHandler
{
    public static Role GetRole(string role)
        => role switch
        {
            "Medic" => Role.Doctor,
            "Client" => Role.Patient,
            _ => Role.Invalid
        };
}