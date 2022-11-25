namespace SiteProject.Models;

public enum BloodType
{
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
            _ => throw new ArgumentOutOfRangeException(nameof(bloodType), bloodType, null)
        };
}