using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Database;

namespace Forms;

internal static class ConnectinSetting
{
    private static readonly DbContextOptions<DataContext> _options = GetOptions();
    public static DbContextOptions<DataContext> Options { get { return _options; } }

    private static DbContextOptions<DataContext> GetOptions()
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("appsettings.json");
        var config = builder.Build();
        var connectionString = config.GetConnectionString("DefaultConnection");
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

        return optionsBuilder.UseNpgsql(connectionString).Options;
    }
}
