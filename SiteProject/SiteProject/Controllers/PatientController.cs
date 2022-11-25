using SiteProject.DataTransfering;
using SiteProject.Models;
using SiteProject.ORM;

namespace SiteProject.Controllers;

public class PatientController
{
    private static Dao<Patient> patientDao = DaoFactory.GetDao<Patient>();
    private static Dao<Disease> diseaseDao = DaoFactory.GetDao<Disease>();
    private static Dao<RecipeRequest> rrDao = DaoFactory.GetDao<RecipeRequest>();
    public RecipeRequestCreatingResult AddRecipeRequest(int patientId, int diseaseId)
    {
        var patient = patientDao.SelectById(patientId);
        if (patient == null) return new RecipeRequestCreatingResult(" No such patient exists");
        var disease = diseaseDao.SelectById(diseaseId);
        if (disease == null) return new RecipeRequestCreatingResult("No such disease exists");
        rrDao.Insert(new RecipeRequest(patient, disease));
        return new RecipeRequestCreatingResult(rrDao
            .SelectBy("PatientId", patientId)
            .Last(rr => rr.Disease.Id == diseaseId).Id);
    }
}