namespace Database;

internal struct EntityInfo(string propertyName, string columnName, string? type)
{
    internal string PropertyName = propertyName;
    internal string ColumnName = columnName;
    internal string? Type = type;
}
