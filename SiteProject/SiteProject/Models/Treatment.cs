using SiteProject.Attributes;
using SiteProject.ORM;

namespace SiteProject.Models;

public class Treatment
{
    private static readonly Dao<Doctor> DoctorDao = DaoFactory.GetDao<Doctor>();
    private static readonly Dao<Drug> DrugDao = DaoFactory.GetDao<Drug>();
    private static readonly Dao<RecipeRequest> RrDao = DaoFactory.GetDao<RecipeRequest>();

    [DbRecordCtor]
    public Treatment(int id, int recipeRequestId, int drugId, int doctorId)
    {
        Id = id;
        RecipeRequest = RrDao.SelectNotNullById(recipeRequestId);
        Drug = DrugDao.SelectNotNullById(drugId);
        Doctor = DoctorDao.SelectNotNullById(doctorId);
    }

    public Treatment(RecipeRequest recipeRequest, Drug drug, Doctor doctor)
    {
        RecipeRequest = recipeRequest;
        Drug = drug;
        Doctor = doctor;
    }

    [Id("Id")] public int Id { get; set; }
    [ValueColumn("RecipeRequestId")] public RecipeRequest RecipeRequest { get; set; }
    [ValueColumn("DoctorId")] public Doctor Doctor { get; set; }
    [ValueColumn("DrugId")] public Drug Drug { get; set; }
}