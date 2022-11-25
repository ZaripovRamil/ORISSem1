using HttpServerTask.Attributes;
using SiteProject.ORM;

namespace SiteProject.Models;

public class Patient
{
    [DbRecordCtor]
    public Patient(int id, string fullName, string age, string bloodType, int diseaseId, int caringDoctorId)
    {
        Id = id;
        FullName = fullName;
        Age = age;
        BloodType = BloodTypeHandler.GetBloodType(bloodType);
        Disease = DaoFactory.GetDao<Disease>().SelectById(diseaseId);
        CaringDoctor = DaoFactory.GetDao<MedicPersonal>().SelectById(caringDoctorId);
    }
    public Patient(string fullName, string age, string bloodType, Disease disease, MedicPersonal caringDoctor)
    {
        FullName = fullName;
        Age = age;
        BloodType = BloodTypeHandler.GetBloodType(bloodType);
        Disease = disease;
        CaringDoctor = caringDoctor;
    }
    public Patient(string fullName, string age, BloodType bloodType, Disease disease, MedicPersonal caringDoctor)
    {
        FullName = fullName;
        Age = age;
        BloodType = bloodType;
        Disease = disease;
        CaringDoctor = caringDoctor;
    }

    [Id("Id")]
    public int Id { get; set; }
    [ValueColumn("FullName")]
    public string FullName { get; set; }
    [ValueColumn("Age")]
    public string Age { get; set; }
    [ValueColumn("BloodType")]
    public BloodType BloodType { get; set; }
    [ValueColumn("Disease")]
    public Disease Disease { get; set; }
    [ValueColumn("CaringDoctor")]
    public MedicPersonal CaringDoctor { get; set; }
    public override string ToString()
    {
        return Id.ToString();
    }
}