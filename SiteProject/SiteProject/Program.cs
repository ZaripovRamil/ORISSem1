// See https://aka.ms/new-console-template for more information

using SiteProject.Models;
using SiteProject.ORM;

DaoFactory.ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Initial Catalog = SiteDb;";
var d = DaoFactory.GetDao<MedicSpecialization>().SelectById(2);
Console.WriteLine(d.SpecName);
