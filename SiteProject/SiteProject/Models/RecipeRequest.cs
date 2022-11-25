using SiteProject.Attributes;
using SiteProject.ORM;

namespace SiteProject.Models;

public class RecipeRequest
{
    [Id("Id")]
    public int Id { get; set; }
    [ValueColumn("PatientId")]
    public Patient Patient { get; set; }
    [ValueColumn("DiseaseId")]
    public Disease Disease { get; set; }
    [ValueColumn("DoctorId")]
    public Doctor Doctor { get; set; }

    [DbRecordCtor]
    public RecipeRequest(int id, int patientId, int diseaseId, int doctorId)
    {
        Id = id;
        Patient = DaoFactory.GetDao<Patient>().SelectById(patientId);
        Disease = DaoFactory.GetDao<Disease>().SelectById(diseaseId);
        Doctor = DaoFactory.GetDao<Doctor>().SelectById(doctorId);
    }

    public RecipeRequest(int patientId, int diseaseId)
    {
        Patient = DaoFactory.GetDao<Patient>().SelectById(patientId);
        Disease = DaoFactory.GetDao<Disease>().SelectById(diseaseId);
        Doctor = Patient.CaringDoctor;
    }
    
    public RecipeRequest(Patient patient, Disease disease)
    {
        Patient = patient;
        Disease = disease;
        Doctor = Patient.CaringDoctor;
    }

    public override string ToString()
    {
        return Id.ToString();
    }
}