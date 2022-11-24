using HttpServerTask.Attributes;
using SiteProject.ORM;

namespace SiteProject.Models;

public class MedicPersonal
{
    [DbRecordCtor]
    public MedicPersonal(int id, string fullName, int specId, int experienceYears)
    {
        Id = id;
        FullName = fullName;
        Spec = DaoFactory.GetDao<MedicSpecialization>().SelectById(specId);
        ExperienceYears = experienceYears;
    }
    
    public MedicPersonal(string fullName, MedicSpecialization spec, int experienceYears)
    {
        FullName = fullName;
        Spec = spec;
        ExperienceYears = experienceYears;
    }

    [Id("Id")]
    public int Id { get; set; }
    [ValueColumn("FullName")]
    public string FullName { get; set; }
    [ValueColumn("SpecId")]
    public MedicSpecialization Spec { get; set; }
    [ValueColumn("ExpYears")]
    public int ExperienceYears { get; set; }
    public override string ToString()
    {
        return Id.ToString();
    }
}