using Microsoft.EntityFrameworkCore;
using Entities;

namespace Database;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ChangeEntityForDatabase<Employee>(modelBuilder);
    }

    private static void ChangeEntityForDatabase<TEntity>(ModelBuilder modelBuilder) where TEntity : BaseEntity
    {
        modelBuilder.Entity<TEntity>().ToTable(ConfigureDatabase.GetTableName<TEntity>(), schema: ConfigureDatabase.Schema);

        foreach (var entityInfo in ConfigureDatabase.GetEntityInfo<TEntity>())
        {
            modelBuilder.Entity<TEntity>().Property(entityInfo.PropertyName).HasColumnName(entityInfo.ColumnName);

            if(entityInfo.ColumnType is not null)
                modelBuilder.Entity<TEntity>().Property(entityInfo.PropertyName).HasColumnType(entityInfo.ColumnType);
        }
    }
}
