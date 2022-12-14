using Scriban;
using SiteProject.Attributes;
using SiteProject.ORM;

namespace SiteProject.Models;

public class Doctor : Person
{
    [DbRecordCtor]
    public Doctor(int id, string fullName, int specId, int experienceYears) : base(id, fullName)
    {
        Spec = DaoFactory.GetDao<MedicSpecialization>().SelectNotNullById(specId);
        ExperienceYears = experienceYears;
    }

    [ValueColumn("SpecId")] public MedicSpecialization Spec { get; set; }
    [ValueColumn("ExperienceYears")] public int ExperienceYears { get; set; }

    public static string GenerateProfileHtml(Template template, Doctor doctor)
    {
        return template.Render(
            new
            {
                doctor_name = doctor.FullName,
                spec = doctor.Spec.SpecName,
                exp_years = doctor.ExperienceYears
            }
        );
    }

    public override string ToString()
    {
        return Id.ToString();
    }
}