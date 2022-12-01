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

    [ValueColumn("Id")] public int Id { get; set; }
    [ValueColumn("FullName")] public string FullName { get; set; }
    [ValueColumn("Age")] public int Age { get; set; }
    [ValueColumn("BloodType")] public BloodType BloodType { get; set; }

    [ValueColumn("CaringDoctor")]
    public override string ToString()
    {
        return Id.ToString();
    }
}