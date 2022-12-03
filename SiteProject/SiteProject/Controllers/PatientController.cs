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
    private static readonly Dao<PrescriptionRequest> PrDao = DaoFactory.GetDao<PrescriptionRequest>();
    private static readonly Dao<Prescription> PrescriptionDao = DaoFactory.GetDao<Prescription>();

    private static PrescriptionRequestValidationResult ValidatePrescription(int patientId, string diseaseName)
    {
        var patient = PatientDao.SelectById(patientId);
        if (patient == null) return new PrescriptionRequestValidationResult(" No such patient exists");
        var disease = DiseaseDao.SelectBy("Name", diseaseName).FirstOrDefault();
        if (disease == null) return new PrescriptionRequestValidationResult("No such disease exists");
        PrDao.Insert(new PrescriptionRequest(patient, disease));
        return new PrescriptionRequestValidationResult(PrDao
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

    [HttpPOST]
    public RequestResult RecipeRequestAddingAttempt(string disease, int userId)
    {
        var res = ValidatePrescription(userId, disease);
        if (!res.IsValid) return OpenView(res.Message, userId);
        return OpenView("New prescription request has been sent", userId);
    }

    protected override RequestResult OpenView(string message, int userId)
    {
        var doctorTemplate = Template.Parse(File.ReadAllText("Views/doctor.html"));
        var prTemplate = Template.Parse(File.ReadAllText("Views/prescription.html"));
        var drugTemplate = Template.Parse(File.ReadAllText("Views/drug.html"));
        var template = Template.Parse(File.ReadAllText("Views/patient_menu.html"));
        var diseases = DiseaseDao.Select().Select(d => d.Name);
        var res = GenerateHtml(template, prTemplate, doctorTemplate, diseases, message, userId, drugTemplate);
        return new RequestResult(200, "text/html", res);
    }

    private static byte[] GenerateHtml(Template template, Template prTemplate, Template doctorTemplate,
        IEnumerable<string> diseases, string message, int userId, Template drugTemplate)
    {
        return Encoding.UTF8.GetBytes(
            template.Render(
                new
                {
                    message = message,
                    diseases = diseases,
                    prescriptions = GeneratePrescriptions(prTemplate,doctorTemplate,userId, drugTemplate)
                }));
    }

    private static IEnumerable<string> GeneratePrescriptions(Template prTemplate, Template doctorTemplate, int userId,
        Template drugTemplate)
    {
        return PrDao
            .SelectBy("PatientId", userId)
            .Select(pr => (pr,
                PrescriptionDao
                    .SelectBy("PrId", pr.Id)
                    .FirstOrDefault())
            ).Where(p => p.Item2 != null)
            .Select(p => prTemplate.Render(
                new
                {
                    prescription_id = p.Item2!.Id,
                    patient_name = p.pr.Patient.FullName,
                    disease = p.pr.Disease.Name,
                    drug_data = Drug.GenerateHtml(drugTemplate, p.Item2.Drug),
                    doctor_data = Doctor.GenerateProfileHtml(doctorTemplate, p.Item2.Doctor)
                }));
    }

    [HttpGET]
    public override RequestResult GetUserMenu(int userId) => base.GetUserMenu(userId);
}