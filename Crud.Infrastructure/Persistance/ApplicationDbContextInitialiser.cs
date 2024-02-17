

using Azure.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Crud.Infrastructure.Persistance;

public class ApplicationDbContextInitialiser
{
    private readonly ApplicationDbContext _context;


    public ApplicationDbContextInitialiser(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
}