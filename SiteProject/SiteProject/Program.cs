// See https://aka.ms/new-console-template for more information

using SiteProject.Models;
using SiteProject.ORM;

DaoFactory.ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Initial Catalog = SiteDb;";
DaoFactory.GetDao<User>().Insert(new User("Login1", "Password1".GetHashCode(), Role.Client));
var user = DaoFactory.GetDao<User>().SelectById(1);
Console.WriteLine(user.Role);
