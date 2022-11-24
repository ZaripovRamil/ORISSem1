﻿using HttpServerTask.Attributes;

namespace SiteProject.Models;

public class User
{
    [Id("Id")]
    public int Id { get; set; }
    [ValueColumn("Login")]
    public string Login { get; set; }
    [ValueColumn("Password")]
    public int Password { get; set; }
    [ValueColumn("Role")]
    public Role Role { get; set; }
    public User(string login, int password, Role role)
    {
        Role = role;
        Login = login;
        Password = password;
    }
    [DbRecordCtor]
    public User(int id, string login, int password, Role role)
    {
        Id = id;
        Login = login;
        Password = password;
        Role = role;
    }
    public override string ToString()
    {
        return Id.ToString();
    }
}
