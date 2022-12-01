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
            "Doctor" => Role.Doctor,
            "Patient" => Role.Patient,
            _ => Role.Invalid
        };
}