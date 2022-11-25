namespace SiteProject.Attributes;

public abstract class Column:Attribute
{
    public readonly string ColumnName;

    protected Column(string columnName)
    {
        ColumnName = columnName;
    }
}