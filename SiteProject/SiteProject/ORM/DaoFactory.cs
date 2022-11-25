namespace SiteProject.ORM;

public static class DaoFactory
{
    private static readonly HashSet<object> Daos = new();
    public static string ConnectionString { get; set; }

    public static Dao<T> GetDao<T>()
    {
        var cachedDao = Daos.FirstOrDefault(obj => obj is Dao<T>);
        if (cachedDao != null)
            return (Dao<T>) cachedDao;
        var dao = new Dao<T>(ConnectionString);
        Daos.Add(dao);
        return dao;
    }
}