using SiteProject.Attributes;
using SiteProject.ORM;

namespace SiteProject.Models;

public class Patient
{
    [DbRecordCtor]
    public Patient(int id, string fullName, string age, string bloodType, int caringDoctorId)
    {
        Id = id;
        FullName = fullName;
        Age = age;
        BloodType = BloodTypeHandler.GetBloodType(bloodType);
        CaringDoctor = DaoFactory.GetDao<Doctor>().SelectById(caringDoctorId);
    }
    public Patient(string fullName, string age, string bloodType, Doctor caringDoctor)
    {
        FullName = fullName;
        Age = age;
        BloodType = BloodTypeHandler.GetBloodType(bloodType);
        CaringDoctor = caringDoctor;
    }
    public Patient(string fullName, string age, BloodType bloodType, Doctor caringDoctor)
    {
        FullName = fullName;
        Age = age;
        BloodType = bloodType;
        CaringDoctor = caringDoctor;
    }

    [ValueColumn("Id")]
    public int Id { get; set; }
    [ValueColumn("FullName")]
    public string FullName { get; set; }
    [ValueColumn("Age")]
    public string Age { get; set; }
    [ValueColumn("BloodType")]
    public BloodType BloodType { get; set; }
    [ValueColumn("CaringDoctor")]
    public Doctor CaringDoctor { get; set; }
    public override string ToString()
    {
        return Id.ToString();
    }
}