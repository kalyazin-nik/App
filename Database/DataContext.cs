using Microsoft.EntityFrameworkCore;
using Entities;

namespace Database;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        AssignNameToTablesAndColumns<Employee>(modelBuilder);
    }

    private static void AssignNameToTablesAndColumns<TEntity>(ModelBuilder modelBuilder) where TEntity : BaseEntity
    {
        var schema = ConfigureDatabase.Schema;
        var tableName = ConfigureDatabase.TableNames[typeof(TEntity).Name];
        var columnNames = ConfigureDatabase.ColumnNames;

        modelBuilder.Entity<TEntity>().ToTable(tableName, schema: schema);
        var properties = typeof(TEntity).GetProperties();

        foreach (var property in properties.Select(p => p).Where(p => columnNames.ContainsKey(p.Name)))
                modelBuilder.Entity<TEntity>().Property(property.Name).HasColumnName(columnNames[property.Name]);
    }
}
