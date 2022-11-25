using SiteProject.Attributes;
using SiteProject.ORM;

namespace SiteProject.Models;

public class Doctor
{
    [DbRecordCtor]
    public Doctor(int id, string fullName, int specId, int experienceYears)
    {
        Id = id;
        FullName = fullName;
        Spec = DaoFactory.GetDao<MedicSpecialization>().SelectNotNullById(specId);
        ExperienceYears = experienceYears;
    }

    [ValueColumn("Id")] public int Id { get; set; }
    [ValueColumn("FullName")] public string FullName { get; set; }
    [ValueColumn("SpecId")] public MedicSpecialization Spec { get; set; }
    [ValueColumn("ExperienceYears")] public int ExperienceYears { get; set; }

    public override string ToString()
    {
        return Id.ToString();
    }
}