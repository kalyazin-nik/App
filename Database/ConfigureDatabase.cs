using System.Reflection;
using Entities;

namespace Database;

public static class ConfigureDatabase
{
    private readonly static string _schema = "company";
    private readonly static Dictionary<string, string> _tableNames = new()
    {
        { "Employee", "employees" }
    };
    private readonly static Dictionary<string, string> _columnNames = new()
    {
        { "Id", "id" },
        { "Post", "post" },
        { "LastName", "last_name" },
        { "FirstName", "first_name" },
        { "MiddleName", "middle_name" },
        { "Age", "age" },
        { "IsMarried", "is_married" },
        { "Address", "address" },
        { "City", "city" },
        { "PhoneNumber", "phone_number" },
        { "Mail", "mail" },
        { "Hobbies", "hobbies" },
        { "CreatedAt", "created_at" }
    };

    internal static string Schema { get { return _schema; } }

    internal static string GetTableName<TEntity>() where TEntity : BaseEntity
    {
        return _tableNames[typeof(TEntity).Name];
    }

    public static Dictionary<string, string> GetColumnNames(PropertyInfo[] properties)
    {
        return properties
            .Select(p => p)
            .Where(p => _columnNames.ContainsKey(p.Name))
            .ToDictionary(p => p.Name, p => _columnNames[p.Name]);
    }
}
