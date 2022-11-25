using SiteProject.Attributes;
using SiteProject.ORM;

namespace SiteProject.Models;

public class Disease
{
    [DbRecordCtor]
    public Disease(int id, string name, int specId, string description)
    {
        Id = id;
        Spec = DaoFactory.GetDao<MedicSpecialization>().SelectById(specId);
        Name = name;
        Description = description;
    }
    
    public Disease(MedicSpecialization spec, string name, string description)
    {
        Spec = spec;
        Name = name;
        Description = description;
    }

    [Id("Id")]
    public int Id { get; set; }
    [ValueColumn("SpecId")]
    public MedicSpecialization Spec { get; set; }
    [ValueColumn("Name")]
    public string Name { get; set; }
    [ValueColumn("Description")]
    public string Description { get; set; }

    public override string ToString()
    {
        return Id.ToString();
    }
}