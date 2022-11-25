using SiteProject.Attributes;
using SiteProject.ORM;

namespace SiteProject.Models;

public class Patient
{
    [DbRecordCtor]
    public Patient(int id, string fullName, int age, string bloodType)
    {
        Id = id;
        FullName = fullName;
        Age = age;
        BloodType = BloodTypeHandler.GetBloodType(bloodType);
    }

    public Patient(int id, string fullName, int age, BloodType bloodType)
    {
        Id = id;
        FullName = fullName;
        Age = age;
        BloodType = bloodType;
    }

    public Patient(string fullName, int age, string bloodType)
    {
        FullName = fullName;
        Age = age;
        BloodType = BloodTypeHandler.GetBloodType(bloodType);
    }

    public Patient(string fullName, int age, BloodType bloodType)
    {
        FullName = fullName;
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