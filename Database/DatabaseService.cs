namespace Database;

public class DatabaseService(DataContext context)
{
    private readonly DataContext _context = context;
}
