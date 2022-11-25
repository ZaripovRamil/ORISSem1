using SiteProject.ActionResults;
using SiteProject.Models;
using SiteProject.ORM;

namespace SiteProject.Controllers;

public static class PatientController
{
    private static readonly Dao<Patient> PatientDao = DaoFactory.GetDao<Patient>();
    private static readonly Dao<Disease> DiseaseDao = DaoFactory.GetDao<Disease>();
    private static readonly Dao<RecipeRequest> RrDao = DaoFactory.GetDao<RecipeRequest>();
    private static RecipeRequestCreatingResult CreateRecipeRequest(int patientId, string diseaseName)
    {
        var patient = PatientDao.SelectById(patientId);
        if (patient == null) return new RecipeRequestCreatingResult(" No such patient exists");
        var disease = DiseaseDao.SelectBy("Name",diseaseName).FirstOrDefault();
        if (disease == null) return new RecipeRequestCreatingResult("No such disease exists");
        RrDao.Insert(new RecipeRequest(patient, disease));
        return new RecipeRequestCreatingResult(RrDao
            .SelectBy("PatientId", patientId)
            .Last(rr => rr.Disease.Name == diseaseName).Id);
    }
}