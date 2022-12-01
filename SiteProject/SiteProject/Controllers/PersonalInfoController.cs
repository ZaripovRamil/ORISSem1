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

    private static PersonalInfoValidationResult ValidatePatient(int userId, string[] info)
    {
        if (info.Length != 3) return new PersonalInfoValidationResult("Invalid arguments");
        var name = info[0];
        var ageString = info[1];
        var bloodType = BloodTypeHandler.GetBloodType(info[2]);
        if (name == string.Empty) return new PersonalInfoValidationResult("Empty name");
        if (!int.TryParse(ageString, out var age)) return new PersonalInfoValidationResult("Invalid age");
        if (bloodType == BloodType.Invalid)
            return new PersonalInfoValidationResult(" Invalid blood type");
        PatientDao.Insert(new Patient(userId, name, age, bloodType));
        return new PersonalInfoValidationResult(userId);
    }

    private static PersonalInfoValidationResult ValidateDoctor(int userId, string[] info)
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
        DoctorDao.Insert(new Doctor(userId, name, spec.Id, expYears));
        return new PersonalInfoValidationResult(userId);
    }
}