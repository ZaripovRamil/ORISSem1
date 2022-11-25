namespace SiteProject.ORM;

public class Dao<T>
{
    private MyOrm Orm { get; }

    public Dao(string connectionString)
    {
        Orm = new MyOrm(connectionString);
    }

    public IEnumerable<T> Select() => Orm.Select<T>();

    public T? SelectById(int id) => Orm.Select<T>("Id", id).FirstOrDefault();

    public T SelectNotNullById(int id) => Orm.Select<T>("Id", id).First();

    public IEnumerable<T> SelectBy(string column, object value) => Orm.Select<T>(column, value);

    public void Insert(T account) => Orm.Insert(account);

    public void Update(T old, T newAcc) => Orm.Update(old, newAcc);

    public void DeleteById(int id) => Orm.Delete<T>("Id", id);
}