using SiteProject.Attributes;

namespace SiteProject.Models;

public abstract class Person
{
    protected Person(int id, string fullName)
    {
        Id = id;
        FullName = fullName;
    }

    [Id("Id")][ValueColumn("Id")] public int Id { get; set; }
    [ValueColumn("FullName")] public string FullName { get; set; }
}