namespace SiteProject.Models;

public enum BloodType
{
    Invalid = -1,
    O = 0,
    A = 1,
    B = 2,
    AB = 3
}

public static class BloodTypeHandler
{
    public static BloodType GetBloodType(string bloodType)
        => bloodType switch
        {
            "O" => BloodType.O,
            "A" => BloodType.A,
            "B" => BloodType.B,
            "AB" => BloodType.AB,
            _ => BloodType.Invalid
        };
}