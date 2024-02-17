
using Microsoft.EntityFrameworkCore;

namespace Crud.Infrastructure.Persistance;

/// <summary>
/// Initializer class for the application database context to handle database migrations.
/// </summary>
public class ApplicationDbContextInitialiser
{
    private readonly ApplicationDbContext _context;


    public ApplicationDbContextInitialiser(ApplicationDbContext context)
    {
        _context = context;
    }
    /// <summary>
    /// Asynchronously initializes the application database context by applying migrations if using SQL Server.
    /// </summary>
    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                // Apply pending migrations for SQL Server
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
}