using System.Text;
using Scriban;
using SiteProject.ActionResults;
using SiteProject.Attributes;
using SiteProject.Models;
using SiteProject.ORM;

namespace SiteProject.Controllers;

[ApiController("info")]
public static class PersonalInfoController
{
    private static readonly Dao<User> UserDao = DaoFactory.GetDao<User>();
    private static readonly Dao<Patient> PatientDao = DaoFactory.GetDao<Patient>();
    private static readonly Dao<Doctor> DoctorDao = DaoFactory.GetDao<Doctor>();
    private static readonly Dao<MedicSpecialization> MsDao = DaoFactory.GetDao<MedicSpecialization>();

    private static PersonalInfoValidationResult Validate(int userId, params string[] info)
    {
        var user = UserDao.SelectById(userId);
        if (user == null) return new PersonalInfoValidationResult("No such user exists");
        var role = user.Role;
        return role switch
        {
            Role.Patient => ValidatePatient(userId, info),
            Role.Doctor => ValidateDoctor(userId, info),
            _ => new PersonalInfoValidationResult("Invalid user role")
        };
    }

    private static PersonalInfoValidationResult ValidatePatient(int userId, params string[] info)
    {
        if (info.Length != 3) return new PersonalInfoValidationResult("Invalid arguments");
        var name = info[0];
        var ageString = info[2];
        var bloodType = BloodTypeHandler.GetBloodType(info[1]);
        if (name == string.Empty) return new PersonalInfoValidationResult("Empty name");
        if (!int.TryParse(ageString, out var age)) return new PersonalInfoValidationResult("Invalid age");
        if (bloodType == BloodType.Invalid)
            return new PersonalInfoValidationResult(" Invalid blood type");
        var patient = new Patient(userId, name, age, bloodType);
        if(PatientDao.SelectById(userId)!=null)
            PatientDao.Update(patient);
        else PatientDao.Insert(patient);
        return new PersonalInfoValidationResult(userId);
    }

    private static PersonalInfoValidationResult ValidateDoctor(int userId, params string[] info)
    {
        if (info.Length != 3) return new PersonalInfoValidationResult("Invalid arguments");
        var name = info[0];
        var spec = MsDao.SelectBy("SpecName", info[1]).FirstOrDefault();
        var expYearsString = info[2];
        if (name == string.Empty) return new PersonalInfoValidationResult("Empty name");
        if (spec == null)
            return new PersonalInfoValidationResult("Invalid specialization");
        if (!int.TryParse(expYearsString, out var expYears))
            return new PersonalInfoValidationResult("Invalid experience");
        var doctor = new Doctor(userId, name, spec.Id, expYears);
        if(DoctorDao.SelectById(userId)!=null)
            DoctorDao.Update(doctor);
        else DoctorDao.Insert(doctor);
        return new PersonalInfoValidationResult(userId);
    }

    [HttpPOST("doctorinfo")]
    public static RequestResult DoctorInfoInputAttempt(string name, string role, string expYearsString, int userId)
    {
        var res = Validate(userId, name, role, expYearsString);
        if(!res.IsValid)return OpenView(res.Message, userId);
        return new RequestResult("http://localhost:6083/" + RoleController.GetRole(userId));
    }
    
    [HttpPOST("patientinfo")]
    public static RequestResult PatientInfoInputAttempt(string name, string bloodType, string ageString, int userId)
    {
        var res = Validate(userId, name, bloodType, ageString);
        if(!res.IsValid)return OpenView(res.Message, userId);
        return new RequestResult("http://localhost:6083/" + RoleController.GetRole(userId));
    }
    [HttpGET]
    public static RequestResult OpenView(int userId)
        => OpenView("", userId);

    private static RequestResult OpenView(string message, int userId)
    {
        if (userId == 0) return new RequestResult("localhost:6083/login");
        var role = RoleController.GetRole(userId);
        var bloodTypes = BloodTypeHandler.GetTypes();
        var specs = MsDao.Select().Select(ms => ms.SpecName);
        var template = Template.Parse(File.ReadAllText("Views/info.sbnhtml"));
        var res = Encoding.UTF8.GetBytes(template.Render(
            new {message = message, role = role.ToString(), bloodtypes = bloodTypes, specs = specs}));
        return new RequestResult(200, "text/html", res);
    }
}