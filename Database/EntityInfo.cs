namespace Database;

internal struct EntityInfo(string propertyName, string columnName, string? columnType)
{
    internal string PropertyName = propertyName;
    internal string ColumnName = columnName;
    internal string? ColumnType = columnType;
}
