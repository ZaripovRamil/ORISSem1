using SiteProject.ActionResults;
using SiteProject.Models;
using SiteProject.ORM;

namespace SiteProject.Controllers;

public class DoctorController
{
    private static readonly Dao<Doctor> DoctorDao = DaoFactory.GetDao<Doctor>();
    private static readonly Dao<Drug> DrugDao = DaoFactory.GetDao<Drug>();
    private static readonly Dao<RecipeRequest> RrDao = DaoFactory.GetDao<RecipeRequest>();
    private static readonly Dao<Treatment> TreatmentDao = DaoFactory.GetDao<Treatment>();

    private static TreatmentCreatingResult CreateTreatment(int doctorId, int recipeRequestId, string drugName)
    {
        var doctor = DoctorDao.SelectById(doctorId);
        if (doctor == null) return new TreatmentCreatingResult("No such doctor exists");
        var recipeRequest = RrDao.SelectById(recipeRequestId);
        if (recipeRequest == null) return new TreatmentCreatingResult("No such request exists");
        var drug = DrugDao.SelectBy("Name", drugName).FirstOrDefault();
        if (drug == null) return new TreatmentCreatingResult("No such drug exists");
        TreatmentDao.Insert(new Treatment(recipeRequest, drug, doctor));
        return new TreatmentCreatingResult(TreatmentDao.SelectBy("DoctorId", doctorId)
            .Last(rr => rr.Drug.Name == drugName).Id);
    }
}