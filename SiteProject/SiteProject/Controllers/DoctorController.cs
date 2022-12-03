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
        var patientTemplate = Template.Parse(File.ReadAllText("Views/patient.html"));
        var prTemplate = Template.Parse(File.ReadAllText("Views/prescription-request.html"));
        var doctor = DoctorDao.SelectNotNullById(userId);
        var drugs = DrugDao.Select().Where(d=>d.Spec.Id==doctor.Spec.Id).Select(d => d.Name);
        var template = Template.Parse(File.ReadAllText("Views/doctor_menu.html"));
        var res = GenerateHtml(template, prTemplate, patientTemplate, doctor, drugs, message);
        return new RequestResult(200, "text/html", res);
    }

    private static byte[] GenerateHtml(Template template, Template prTemplate, Template patientTemplate, Doctor doctor,
        IEnumerable<string> drugs, string message)
    {
        return Encoding.UTF8.GetBytes(template.Render(
            new
            {
                message = message,
                requests = GeneratePrescriptionRequests(prTemplate, patientTemplate, doctor, drugs)
            }));
    }

    private static IEnumerable<string> GeneratePrescriptionRequests(Template prTemplate, Template patientTemplate,
        Doctor doctor, IEnumerable<string> drugs)
    {
        return PrDao
            .Select()
            .Where(pr => PrescriptionDao
                .SelectBy("PrId", pr.Id)
                .FirstOrDefault() == null)
            .Where(pr => pr.Disease.Spec.Id == doctor.Spec.Id)
            .Select(pr => prTemplate.Render(
                new
                {
                    drugs = drugs,
                    disease = pr.Disease.Name,
                    request_id = pr.Id.ToString(),
                    patient_data = Patient.GenerateHtml(patientTemplate, pr.Patient)
                }))
            .ToArray();
    }

    [HttpGET]
    public override RequestResult GetUserMenu(int userId) => base.GetUserMenu(userId);
}