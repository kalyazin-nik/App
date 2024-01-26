using System.Configuration;

namespace Database;

internal static class ConfigureDatabase
{
    internal readonly static string ConnectionString = ConfigurationManager.ConnectionStrings["postgres"].ConnectionString;
    internal readonly static string Schema = "company";
    internal readonly static Dictionary<string, string> TableNames = new()
    {
        { "Employee", "employees" }
    };
    internal readonly static Dictionary<string, string> ColumnNames = new()
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
}
