namespace SiteProject.ORM;

public class Dao<T>
{
    private MyOrm Orm { get; }
    public Dao(string connectionString)
    {
        Orm = new MyOrm(connectionString);
    }

    public IEnumerable<T> Select() => Orm.Select<T>();

    public T SelectById(int id) => Orm.Select<T>("Id", id).First();
    
    public T? SelectByUsername(string username) => Orm.Select<T>("username", username).FirstOrDefault();

    public void Insert(T account) => Orm.Insert(account);

    public void Update(T old, T newAcc) => Orm.Update(old, newAcc);

    public void DeleteById(int id) => Orm.Delete<T>("id", id);

    public void DeleteByUsername(string username) => Orm.Delete<T>("username", username);
}