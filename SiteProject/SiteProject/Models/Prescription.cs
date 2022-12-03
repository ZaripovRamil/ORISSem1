using SiteProject.Attributes;
using SiteProject.ORM;

namespace SiteProject.Models;

public class Prescription
{
    private static readonly Dao<Doctor> DoctorDao = DaoFactory.GetDao<Doctor>();
    private static readonly Dao<Drug> DrugDao = DaoFactory.GetDao<Drug>();
    private static readonly Dao<PrescriptionRequest> PrDao = DaoFactory.GetDao<PrescriptionRequest>();

    [DbRecordCtor]
    public Prescription(int id, int recipeRequestId, int drugId, int doctorId)
    {
        Id = id;
        PrescriptionRequest = PrDao.SelectNotNullById(recipeRequestId);
        Drug = DrugDao.SelectNotNullById(drugId);
        Doctor = DoctorDao.SelectNotNullById(doctorId);
    }

    public Prescription(PrescriptionRequest prescriptionRequest, Drug drug, Doctor doctor)
    {
        PrescriptionRequest = prescriptionRequest;
        Drug = drug;
        Doctor = doctor;
    }

    [Id("Id")] public int Id { get; set; }
    [ValueColumn("PrId")] public PrescriptionRequest PrescriptionRequest { get; set; }
    [ValueColumn("DoctorId")] public Doctor Doctor { get; set; }
    [ValueColumn("DrugId")] public Drug Drug { get; set; }
}