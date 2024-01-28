using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Database;

namespace Forms;

internal class DbContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    private static DbContextOptions<DataContext> _options = null!;
    internal static DbContextOptions<DataContext> Options { get { return _options ?? GetOptions(); } }

    private static DbContextOptions<DataContext> GetOptions()
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("appsettings.json");
        var config = builder.Build();
        var connectionString = config.GetConnectionString("DefaultConnection");
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        _options = optionsBuilder.UseNpgsql(connectionString).Options;

        return _options;
    }

    public DataContext CreateDbContext(string[] args)
    {
        return new DataContext(Options);
    }

    internal static DataContext CreateDataContext()
    {
        return new DataContext(Options);
    }
}
