using SiteProject.Attributes;
using SiteProject.ORM;

namespace SiteProject.Models;

public class RecipeRequest
{
    [Id("Id")] public int Id { get; set; }
    [ValueColumn("PatientId")] public Patient Patient { get; set; }
    [ValueColumn("DiseaseId")] public Disease Disease { get; set; }

    [DbRecordCtor]
    public RecipeRequest(int id, int patientId, int diseaseId)
    {
        Id = id;
        Patient = DaoFactory.GetDao<Patient>().SelectNotNullById(patientId);
        Disease = DaoFactory.GetDao<Disease>().SelectNotNullById(diseaseId);
    }

    public RecipeRequest(int patientId, int diseaseId)
    {
        Patient = DaoFactory.GetDao<Patient>().SelectNotNullById(patientId);
        Disease = DaoFactory.GetDao<Disease>().SelectNotNullById(diseaseId);
    }

    public RecipeRequest(Patient patient, Disease disease)
    {
        Patient = patient;
        Disease = disease;
    }

    public override string ToString()
    {
        return Id.ToString();
    }
}