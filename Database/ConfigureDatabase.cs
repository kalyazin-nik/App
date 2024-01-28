using Entities;

namespace Database;

public static class ConfigureDatabase
{
    internal static string Schema { get { return _schema; } }
    private readonly static string _schema = "company";
    private readonly static Dictionary<string, string> _tableNames = new()
    {
        { "Employee", "employees" }
    };
    private readonly static Dictionary<string, EntityInfo> _entityInfo = new()
    {
        { "Id", new("Id", "id", null) },
        { "Post", new("Post", "post", "varchar(50)") },
        { "Surname", new("Surname", "surname", "varchar(20)") },
        { "Name", new("Name", "name", "varchar(20)") },
        { "Patronymic", new("Patronymic", "patronymic", "varchar(20)") },
        { "DateOfBirth", new("DateOfBirth", "date_of_birth", "date") },
        { "PhoneNumber", new("PhoneNumber", "phone_number", "char(16)") },
        { "Mail", new("Mail", "mail", "varchar(70)") },
        { "FamilyStatus", new("FamilyStatus", "family_status", "varchar(20)") },
        { "Address", new("Address", "address", "text") },
        { "City", new("City", "city", "varchar(20)") },
        { "Hobbies", new("Hobbies", "hobbies", "varchar(20)[]") },
        { "CreatedAt", new("CreatedAt", "created_at", "timestamp") }
    };

    internal static string GetTableName<TEntity>() where TEntity : BaseEntity
    {
        return _tableNames[typeof(TEntity).Name];
    }

    internal static IEnumerable<EntityInfo> GetEntityInfo<TEntity>() where TEntity : BaseEntity
    {
        foreach (var property in typeof(TEntity).GetProperties())
            if (_entityInfo.TryGetValue(property.Name, out EntityInfo value))
                yield return value;
    }
}
