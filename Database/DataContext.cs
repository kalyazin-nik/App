using Microsoft.EntityFrameworkCore;
using Entities;

namespace Database;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        RenameTablesAndColumns<Employee>(modelBuilder);
    }

    private static void RenameTablesAndColumns<TEntity>(ModelBuilder modelBuilder) where TEntity : BaseEntity
    {
        var schema = ConfigureDatabase.Schema;
        var tableName = ConfigureDatabase.GetTableName<TEntity>();
        var columnNames = ConfigureDatabase.GetColumnNames(typeof(TEntity).GetProperties());

        modelBuilder.Entity<TEntity>().ToTable(tableName, schema: schema);

        foreach (var pair in columnNames)
                modelBuilder.Entity<TEntity>().Property(pair.Key).HasColumnName(pair.Value);
    }
}
