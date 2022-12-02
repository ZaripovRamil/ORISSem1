using SiteProject.Attributes;
using SiteProject.ORM;

namespace SiteProject.Models;

public class Patient:Person
{
    [DbRecordCtor]
    public Patient(int id, string fullName, int age, string bloodType):base(id, fullName)
    {
        Age = age;
        BloodType = BloodTypeHandler.GetBloodType(bloodType);
    }

    public Patient(int id, string fullName, int age, BloodType bloodType):base(id, fullName)
    {
        Age = age;
        BloodType = bloodType;
    }
    [ValueColumn("Age")] public int Age { get; set; }
    [ValueColumn("BloodType")] public BloodType BloodType { get; set; }
    public override string ToString()
    {
        return Id.ToString();
    }
}