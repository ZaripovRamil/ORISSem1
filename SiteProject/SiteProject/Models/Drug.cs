using HttpServerTask.Attributes;
using SiteProject.ORM;

namespace SiteProject.Models;

public class Drug
{
    [DbRecordCtor]
    public Drug(int id, string name, int specId, string impactDesc, string useInstruction, int price)
    {
        Id = id;
        Name = name;
        Spec = DaoFactory.GetDao<MedicSpecialization>().SelectById(specId);
        ImpactDesc = impactDesc;
        UseInstruction = useInstruction;
        Price = price;
    }

    [DbRecordCtor]
    public Drug(string name, MedicSpecialization spec, string impactDesc, string useInstruction, int price)
    {
        Name = name;
        Spec = spec;
        ImpactDesc = impactDesc;
        UseInstruction = useInstruction;
        Price = price;
    }

    [Id("Id")] public int Id { get; set; }
    [ValueColumn("Name")] public string Name { get; set; }
    [ValueColumn("SpecId")] public MedicSpecialization Spec { get; set; }
    [ValueColumn("ImpactDesc")] public string ImpactDesc { get; set; }
    [ValueColumn("Instruction")] public string UseInstruction { get; set; }
    [ValueColumn("Price")] public int Price { get; set; }
    public override string ToString()
    {
        return Id.ToString();
    }
}