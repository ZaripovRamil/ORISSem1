using System.Text;
using Scriban;
using SiteProject.ActionResults;
using SiteProject.Attributes;
using SiteProject.Models;
using SiteProject.ORM;

namespace SiteProject.Controllers;

[ApiController("Doctor")]
public class DoctorController:RoleController
{
    private static readonly Dao<Doctor> DoctorDao = DaoFactory.GetDao<Doctor>();
    private static readonly Dao<Drug> DrugDao = DaoFactory.GetDao<Drug>();
    private static readonly Dao<RecipeRequest> RrDao = DaoFactory.GetDao<RecipeRequest>();
    private static readonly Dao<Treatment> TreatmentDao = DaoFactory.GetDao<Treatment>();

    public DoctorController()
    {
    }

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

    protected override bool IsCorrectRole(int userId)
    {
        return GetRole(userId) == Role.Doctor;
    }

    protected override bool IsInfoFilled(int userId)
    {
        return DoctorDao.SelectById(userId) != null;
    }

    protected override RequestResult OpenView(int userId)
    {
        var template = Template.Parse(File.ReadAllText("Views/doctor.sbnhtml"));
        var res = Encoding.UTF8.GetBytes(template.Render(new {message = userId}));
        return new RequestResult(200, "text/html", res);
    }

    [HttpGET]
    public override RequestResult GetUserMenu(int userId) => base.GetUserMenu(userId);
}