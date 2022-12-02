using System.Text;
using Scriban;
using SiteProject.ActionResults;
using SiteProject.Attributes;
using SiteProject.Models;
using SiteProject.ORM;

namespace SiteProject.Controllers;

[ApiController("Patient")]
public class PatientController : RoleController
{
    private static readonly Dao<Patient> PatientDao = DaoFactory.GetDao<Patient>();
    private static readonly Dao<Disease> DiseaseDao = DaoFactory.GetDao<Disease>();
    private static readonly Dao<RecipeRequest> RrDao = DaoFactory.GetDao<RecipeRequest>();

    private static RecipeRequestCreatingResult CreateRecipeRequest(int patientId, string diseaseName)
    {
        var patient = PatientDao.SelectById(patientId);
        if (patient == null) return new RecipeRequestCreatingResult(" No such patient exists");
        var disease = DiseaseDao.SelectBy("Name", diseaseName).FirstOrDefault();
        if (disease == null) return new RecipeRequestCreatingResult("No such disease exists");
        RrDao.Insert(new RecipeRequest(patient, disease));
        return new RecipeRequestCreatingResult(RrDao
            .SelectBy("PatientId", patientId)
            .Last(rr => rr.Disease.Name == diseaseName).Id);
    }

    protected override bool IsCorrectRole(int userId)
    {
        return GetRole(userId) == Role.Patient;
    }

    protected override bool IsInfoFilled(int userId)
    {
        return PatientDao.SelectById(userId) != null;
    }

    protected override RequestResult OpenView(int userId)
    {
        var template = Template.Parse(File.ReadAllText("Views/patient.sbnhtml"));
        var res = Encoding.UTF8.GetBytes(template.Render(new {id = userId.ToString()}));
        return new RequestResult(200, "text/html", res);
    }

    [HttpGET]
    public override RequestResult GetUserMenu(int userId) => base.GetUserMenu(userId);
}