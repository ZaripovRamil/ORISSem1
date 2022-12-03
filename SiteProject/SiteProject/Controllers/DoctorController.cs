using System.Text;
using Scriban;
using SiteProject.ActionResults;
using SiteProject.Attributes;
using SiteProject.Models;
using SiteProject.ORM;

namespace SiteProject.Controllers;

[ApiController("Doctor")]
public class DoctorController : RoleController
{
    private static readonly Dao<Doctor> DoctorDao = DaoFactory.GetDao<Doctor>();
    private static readonly Dao<Drug> DrugDao = DaoFactory.GetDao<Drug>();
    private static readonly Dao<PrescriptionRequest> PrDao = DaoFactory.GetDao<PrescriptionRequest>();
    private static readonly Dao<Prescription> PrescriptionDao = DaoFactory.GetDao<Prescription>();

    private static PrescriptionValidationResult ValidatePrescription(int doctorId, int recipeRequestId, string drugName)
    {
        var doctor = DoctorDao.SelectById(doctorId);
        if (doctor == null) return new PrescriptionValidationResult("No such doctor exists");
        var recipeRequest = PrDao.SelectById(recipeRequestId);
        if (recipeRequest == null) return new PrescriptionValidationResult("No such request exists");
        var drug = DrugDao.SelectBy("Name", drugName).FirstOrDefault();
        if (drug == null) return new PrescriptionValidationResult("No such drug exists");
        PrescriptionDao.Insert(new Prescription(recipeRequest, drug, doctor));
        return new PrescriptionValidationResult(PrescriptionDao.SelectBy("DoctorId", doctorId)
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

    [HttpPOST]
    public RequestResult PrescriptionAddingAttempt(int prId, string drugName, int userId)
    {
        var res = ValidatePrescription(userId, prId, drugName);
        if (!res.IsValid) return OpenView(res.Message, userId);
        return OpenView("New prescription request has been sent", userId);
    }

    protected override RequestResult OpenView(string message, int userId)
    {
        var prTemplate = Template.Parse(File.ReadAllText("Views/prescription-request.sbnhtml"));
        var doctor = DoctorDao.SelectNotNullById(userId);
        var drugs = DrugDao.Select().Select(d => d.Name);
        var prHtmls =
            PrDao
                .Select()
                .Where(pr => PrescriptionDao
                    .SelectBy("PrId", pr.Id)
                    .FirstOrDefault() == null)
                .Where(pr => pr.Disease.Spec.Id == doctor.Spec.Id)
                .Select(pr => prTemplate.Render(
                    new
                    {
                        drugs=drugs,
                        disease = pr.Disease.Name,
                        request_id = pr.Id.ToString(),
                        patient_name = pr.Patient.FullName
                    }))
                .ToArray();
        var template = Template.Parse(File.ReadAllText("Views/doctor.sbnhtml"));
        var res = Encoding.UTF8.GetBytes(template.Render(new {message = message, requests = prHtmls}));
        return new RequestResult(200, "text/html", res);
    }

    [HttpGET]
    public override RequestResult GetUserMenu(int userId) => base.GetUserMenu(userId);
}