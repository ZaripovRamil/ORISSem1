using SiteProject.Attributes;
using SiteProject.ORM;

namespace SiteProject.Models;

public class PrescriptionRequest
{
    [Id("Id")] public int Id { get; set; }
    [ValueColumn("PatientId")] public Patient Patient { get; set; }
    [ValueColumn("DiseaseId")] public Disease Disease { get; set; }

    [DbRecordCtor]
    public PrescriptionRequest(int id, int patientId, int diseaseId)
    {
        Id = id;
        Patient = DaoFactory.GetDao<Patient>().SelectNotNullById(patientId);
        Disease = DaoFactory.GetDao<Disease>().SelectNotNullById(diseaseId);
    }

    public PrescriptionRequest(int patientId, int diseaseId)
    {
        Patient = DaoFactory.GetDao<Patient>().SelectNotNullById(patientId);
        Disease = DaoFactory.GetDao<Disease>().SelectNotNullById(diseaseId);
    }

    public PrescriptionRequest(Patient patient, Disease disease)
    {
        Patient = patient;
        Disease = disease;
    }

    public override string ToString()
    {
        return Id.ToString();
    }
}