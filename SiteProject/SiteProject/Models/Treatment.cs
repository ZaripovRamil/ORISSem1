using HttpServerTask.Attributes;
using SiteProject.ORM;

namespace SiteProject.Models;

public class Treatment
{
    [DbRecordCtor]
    public Treatment(int id, int patientId, int doctorId, int drugId)
    {
        Id = id;
        Patient = DaoFactory.GetDao<Patient>().SelectById(patientId);
        Doctor = DaoFactory.GetDao<MedicPersonal>().SelectById(doctorId);
        Drug = DaoFactory.GetDao<Drug>().SelectById(drugId);
    }

    [Id("Id")] public int Id { get; set; }
    [ValueColumn("PatientId")] public Patient Patient { get; set; }
    [ValueColumn("DoctorId")] public MedicPersonal Doctor { get; set; }
    [ValueColumn("DrugId")] public Drug Drug { get; set; }
}