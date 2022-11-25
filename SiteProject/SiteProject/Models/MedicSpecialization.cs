using SiteProject.Attributes;

namespace SiteProject.Models;

public class MedicSpecialization
{
    [DbRecordCtor]
    public MedicSpecialization(int id, string specName)
    {
        Id = id;
        SpecName = specName;
    }

    public MedicSpecialization(string specName)
    {
        SpecName = specName;
    }

    [Id("Id")] public int Id { get; set; }
    [ValueColumn("SpecName")] public string SpecName { get; set; }

    public override string ToString()
    {
        return Id.ToString();
    }
}